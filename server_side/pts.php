<?php

	//foreach ($_POST as $key => $value) {
	//	echo '<p>' . $key . ' => ' . $value . '</p>';
	//}
	//echo ($_POST['jsondata']);
	function user_exists($login)
	{
		$res = mysql_query("select * from users where login='" . $login . "';")
		if (!$res || mysql_num_rows($res) == 0)
			return 0;
		return 1;
	}

	if (isset($_POST['referer']))
	{
		$mysql_conn = mysql_connect('localhost:3306', 'admin', '');
		if (!$mysql_conn)
			die('error: ' . mysql_error());
		$db = mysql_select_db('dbname');
		if (!$db)
			die('error: ' . mysql_error());

		$login = $_POST['user'];
		$filename = 'files/' . $login . '.txt';
		if ($_POST['referer'] == 'desktop')
		{
			//create and write / re-write file with jsondata
			
			if (user_exists($login) == 0)
			{
				$sql = "insert into users values('" . $login . "'," 
					. intval($_POST['password_hash']) . ",'" . $filename . "');";
				$res = mysql_query($sql);
				
				file_put_contents($filename, $_POST['jsondata']); //creates or overwrites file
			}
			else 
			{
				//check pass and re-write
				$sql = "select password_hash from users where login='" . $login . "' limit 1;";
				$res = mysql_query($sql);
				$value = mysql_fetch_object($res);
				if ($value->password_hash == $_POST['password_hash'])
				{
					file_put_contents($filename, $_POST['jsondata']);
				}
				else
					die('error: wrong password');
			}
		}	
		else if ($_POST['referer'] == 'mobile')
		{
			$sql = "select password_hash from users where login='" . $login . "' limit 1;";
			$res = mysql_query($sql);
			if (!$res)
				die('error: no such user');
			$value = mysql_fetch_object($res);
			if ($value->password_hash == md5($_POST['password_hash']))
			{
				$file = file_get_contents($filename);
				echo $file; //echoes json data
			}
		}
		mysql_close();
	}


?>