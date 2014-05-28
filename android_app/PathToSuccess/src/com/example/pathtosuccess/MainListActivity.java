package com.example.pathtosuccess;

import java.io.BufferedReader;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;
import java.io.IOException;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.io.UnsupportedEncodingException;
import java.util.ArrayList;
import java.util.List;

import org.apache.http.HttpEntity;
import org.apache.http.HttpResponse;
import org.apache.http.NameValuePair;
import org.apache.http.client.ClientProtocolException;
import org.apache.http.client.HttpClient;
import org.apache.http.client.entity.UrlEncodedFormEntity;
import org.apache.http.client.methods.HttpPost;
import org.apache.http.impl.client.DefaultHttpClient;
import org.apache.http.message.BasicNameValuePair;
import org.apache.http.protocol.HTTP;

import com.example.pathtosuccess.util.HashUtil;

import android.net.ConnectivityManager;
import android.net.NetworkInfo;
import android.os.AsyncTask;
import android.os.Bundle;
import android.app.Activity;
import android.app.AlertDialog;
import android.app.ListActivity;
import android.content.Context;
import android.content.DialogInterface;
import android.content.Intent;
import android.content.SharedPreferences;
import android.content.SharedPreferences.Editor;
import android.view.LayoutInflater;
import android.view.Menu;
import android.view.MenuItem;
import android.view.View;
import android.view.ViewGroup;
import android.widget.BaseAdapter;
import android.widget.ListView;
import android.widget.ScrollView;
import android.widget.TextView;

public class MainListActivity extends ListActivity {

	public String jsonString;
	public JSONObject jObject;
	public ArrayList<ListItem> items;
	@Override
	protected void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
		setContentView(R.layout.activity_list);
		
		if (!isDataSaved())
			update();
		else
			loadJson();
		loadItems();
		displayList();
	}
	
	private void loadJson() {
		SharedPreferences sp = getSharedPreferences("com.example.pathtosuccess", MODE_PRIVATE);
		jsonString = sp.getString("com.example.pathtosuccess.json", null);
		if (jsonString != null)
			try {
				jObject = new JSONObject(jsonString);
			} catch (JSONException e) {
				e.printStackTrace();
				AlertDialog.Builder b = new AlertDialog.Builder(this);
				b.setTitle("Ошибка");
				b.setMessage("Похоже, сохраненная информация устарела. Требуется обновление.");
				b.setNeutralButton("Закрыть", new DialogInterface.OnClickListener() {
					@Override
					public void onClick(DialogInterface dialog, int which) {
						dialog.cancel();
					}
				});
				b.show();
			}
	}

	public void displayList()
	{
		if (items == null) return;
		ListItemAdapter a = new ListItemAdapter(this, items);
		ItemSectionizer i = new ItemSectionizer();
		SimpleSectionAdapter<ListItem> adapter = new SimpleSectionAdapter<ListItem>(this, a, R.layout.section_header, R.id.title, i);
		setListAdapter(adapter);
	}
	
	public void loadItems()
	{
		if (jObject == null) return;
		ArrayList<ListItem> data = new ArrayList<ListItem>();
		try {
			JSONArray days = jObject.getJSONArray("days");
			for (int i = 0; i < days.length(); i++) {
				JSONObject day = days.getJSONObject(i);
				String section = day.getString("day") + "." + day.getString("month") + "." + day.getString("year");
				JSONArray tasks = day.getJSONArray("steps");
				for (int j = 0; j < tasks.length(); j++) {
					JSONObject task = tasks.getJSONObject(j);
					String desc = task.getString("description");
					String imp = task.getString("importance");
					String fromTime = task.getString("start_time");
					String toTime = task.getString("end_time");
					data.add(new ListItem(desc, fromTime, toTime, imp, section));
				}
			}
			items = data;
		} catch (JSONException e) {
			e.printStackTrace();
		}
	}
	
	public void update()
	{
		GetJsonTask updateTask = new GetJsonTask();
		String[] params = new String[2];
		SharedPreferences sp = getSharedPreferences("com.example.pathtosuccess", MODE_PRIVATE);
		params[0] = sp.getString("com.example.pathtosuccess.login", null);
		String s = sp.getString("com.example.pathtosuccess.password", null);
		params[1] = Integer.toString(HashUtil.getHash(s));
		updateTask.execute(params);
	}
	
	public void save(String response, JSONObject jobj) {
		if (response == null || jobj == null) return;
		jsonString = response;
		jObject = jobj;
		SharedPreferences sp = getSharedPreferences("com.example.pathtosuccess", MODE_PRIVATE);
		Editor e = sp.edit();
		e.putString("com.example.pathtosuccess.json", jsonString);
		e.putBoolean("com.example.pathtosuccess.isdatasaved", true);
		e.commit();
	}

	
	public boolean isDataSaved()
	{
		SharedPreferences sp = getSharedPreferences("com.example.pathtosuccess", MODE_PRIVATE);
		boolean s = sp.getBoolean("com.example.pathtosuccess.isdatasaved", false);
		return s;
	}

	@Override
	public boolean onCreateOptionsMenu(Menu menu) {
		// Inflate the menu; this adds items to the action bar if it is present.
		getMenuInflater().inflate(R.menu.list, menu);
		return true;
	}
	
	public void logout() {
		SharedPreferences sp = getSharedPreferences("com.example.pathtosuccess", MODE_PRIVATE);
		Editor e = sp.edit();
		e.putBoolean("com.example.pathtosuccess.isloggedin", false);
		e.putBoolean("com.example.pathtosuccess.isdatasaved", false);
		e.commit();
		Intent goToLoginPage = new Intent(getApplicationContext(), LoginActivity.class);
		startActivity(goToLoginPage);
	}
	
	private boolean isNetworkAvailable() {
	    ConnectivityManager connectivityManager 
	          = (ConnectivityManager) getSystemService(Context.CONNECTIVITY_SERVICE);
	    NetworkInfo activeNetworkInfo = connectivityManager.getActiveNetworkInfo();
	    return activeNetworkInfo != null && activeNetworkInfo.isConnected();
	}
	
	public void showUpdateStatus(boolean b) {
		AlertDialog.Builder builder = new AlertDialog.Builder(this);
		builder.setTitle("Сервис обновлений");
		if (b) {
			builder.setMessage("Успешно обновлено.");
		} else {
			builder.setMessage("Не удалось соединиться с сервером.");
		}
		builder.setNeutralButton("Закрыть", new DialogInterface.OnClickListener() {
			@Override
			public void onClick(DialogInterface dialog, int which) {
				dialog.cancel();
			}
		});
		builder.show();
	}
	
	@Override
	public boolean onOptionsItemSelected(MenuItem item) {
	    switch (item.getItemId()) {
	    case R.id.logout:
	        logout();
	        return true;
	    case R.id.update:
	    	if (!isNetworkAvailable()) {
				AlertDialog.Builder b = new AlertDialog.Builder(this);
				b.setTitle("Ошибка");
				b.setMessage("Отсутствует интернет-соединение. Попробуйте еще раз.");
				b.setNeutralButton("Выход", new DialogInterface.OnClickListener() {
					@Override
					public void onClick(DialogInterface dialog, int which) {
						dialog.cancel();
					}
				});
				b.show();
			} else {
				update();
			}
	        return true;
	    default:
	        return super.onOptionsItemSelected(item);
	    }
	}
	
	public class GetJsonTask extends AsyncTask<String[], Void, JSONObject> {
		public JSONObject jObj = null;
		public final String url = "http://pts.it-team.in.ua/index.php";
		public String login = null;
		public String passHash = null;
		public InputStream is = null;
		public String response = null;
		@Override
		protected JSONObject doInBackground(String[]... params) {
			String[] p = params[0];
			login = p[0];
			passHash = p[1];
			List<NameValuePair> req = new ArrayList<NameValuePair>();
			req.add(new BasicNameValuePair("user", login)); //login
			req.add(new BasicNameValuePair("password_hash", passHash));
			req.add(new BasicNameValuePair("auth", "false"));
			req.add(new BasicNameValuePair("referer", "mobile"));
		
			HttpClient httpClient = new DefaultHttpClient();
			HttpPost httpPost = new HttpPost(url);
			try {
				httpPost.setEntity(new UrlEncodedFormEntity(req, HTTP.UTF_8));
			} catch (UnsupportedEncodingException e) {
				e.printStackTrace();
			}

			HttpResponse httpResponse = null;
			try {
				httpResponse = httpClient.execute(httpPost);
			} catch (ClientProtocolException e) {
				e.printStackTrace();
			} catch (IOException e) {
				e.printStackTrace();
			}
			HttpEntity httpEntity = httpResponse.getEntity();
			try {
				is = httpEntity.getContent();
			} catch (IllegalStateException e) {
				e.printStackTrace();
			} catch (IOException e) {
				e.printStackTrace();
			}
			try {
	            BufferedReader reader = new BufferedReader(new InputStreamReader(
	                    is, "iso-8859-1"), 8);
	            StringBuilder sb = new StringBuilder();
	            String line = null;
	            while ((line = reader.readLine()) != null) {
	                sb.append(line + "\n");
	            }
	            is.close();
	            response = sb.toString();
	        } catch (Exception e) {
	            e.printStackTrace();
	        }
			try {
				jObj = new JSONObject(response);
			} catch (JSONException e) {
				e.printStackTrace();
			}
			return jObj;
		}
		
		@Override
		protected void onPostExecute(JSONObject result) {
			if (response != null && result != null) {
				save(response, result);
				showUpdateStatus(true);
				loadItems();
				displayList();
			} else {
				showUpdateStatus(false);
			}
		}
	}
	
	public class ListItem {
		private String description;
		private String fromTime;
		private String toTime;
		private String importance;
		private String sectionName;
		
		public ListItem(String description, String fromTime, String toTime,
				String importance, String sectionName) {
			super();
			this.description = description;
			this.fromTime = fromTime;
			this.toTime = toTime;
			this.importance = importance;
			this.sectionName = sectionName;
		}
		
		public String getDescription() {
			return description;
		}
		public void setDescription(String description) {
			this.description = description;
		}
		public String getFromTime() {
			return fromTime;
		}
		public void setFromTime(String fromTime) {
			this.fromTime = fromTime;
		}
		public String getToTime() {
			return toTime;
		}
		public void setToTime(String toTime) {
			this.toTime = toTime;
		}
		public String getImportance() {
			return importance;
		}
		public void setImportance(String importance) {
			this.importance = importance;
		}
		public String getSectionName() {
			return sectionName;
		}
		public void setSectionName(String sectionName) {
			this.sectionName = sectionName;
		}
		
	}
	
	public class ItemSectionizer implements Sectionizer<ListItem> {

		@Override
		public String getSectionTitleForItem(ListItem instance) {
			return instance.getSectionName();
		}
	}
	
	public class ListItemAdapter extends BaseAdapter {

		private ArrayList<ListItem> data;
		private LayoutInflater layoutInflater;
		
		public ListItemAdapter(Context context, ArrayList<ListItem> data) {
			this.data = data;
			this.layoutInflater = LayoutInflater.from(context);
		}
		
		@Override
		public int getCount() {
			return data.size();
		}

		@Override
		public Object getItem(int position) {
			return data.get(position);
		}

		@Override
		public long getItemId(int position) {
			return position;
		}

		@Override
		public View getView(int position, View convertView, ViewGroup parent) {
			ViewHolder v;
			if (convertView == null) {
				convertView = layoutInflater.inflate(R.layout.list_item_layout, null);
				v = new ViewHolder();
				v.descriptionView = (TextView) convertView.findViewById(R.id.descriptionView);
				v.importanceView = (TextView) convertView.findViewById(R.id.importanceView);
				v.fromTimeView = (TextView) convertView.findViewById(R.id.fromTimeView);
				v.toTimeView = (TextView) convertView.findViewById(R.id.toTimeView);
				convertView.setTag(v);
				//assign textviews and set their text later
			} else {
				v = (ViewHolder) convertView.getTag();
			}
			ListItem i = data.get(position);
			v.descriptionView.setText(i.description);
			v.importanceView.setText(i.importance);
			v.fromTimeView.setText("from " + i.fromTime);
			v.toTimeView.setText("to " + i.toTime);
			return convertView;
		}
	}
	
	static class ViewHolder {
		public TextView descriptionView;
		public TextView fromTimeView;
		public TextView toTimeView;
		public TextView importanceView;
	}
}
