<?php

	function user_exists($login)
	{
		$res = mysql_query("select * from users where login='" . $login . "';");
		if (!$res || mysql_num_rows($res) == 0)
		{
			return 0;
		}
		return 1;
	}

	if (isset($_POST['referer']))
	{
		$mysql_conn = mysql_connect('127.0.0.1:3306', 'pts', 'root');
		if (!$mysql_conn)
			die('error: ' . mysql_error());
		$db = mysql_select_db('pts');
		if (!$db)
			die('error: ' . mysql_error());

		$login = $_POST['user'];
		if ($_POST['referer'] == 'desktop')
		{
			if (user_exists($login) == 0) //first-timer, need to create a row in db
			{
				$sql = "insert into users values('" . $login . "'," 
					. intval($_POST['password_hash']) . ",'" . $_POST['jsondata'] . "');";
				$res = mysql_query($sql);
				echo('successfully created for user: ' . $login);
				//file_put_contents($filename, $_POST['jsondata']); //creates or overwrites file
			}
			else //existing user, need to update
			{
				$sql = "select password_hash from users where login='" . $login . "' limit 1;";
				$res = mysql_query($sql);
				$value = mysql_fetch_assoc($res);
				if ($value['password_hash'] == intval($_POST['password_hash']))
				{
					$sql = "update users set jsondata='" . $_POST['jsondata'] . "' where login='" . $login . "';";
					$res = mysql_query($sql);
					echo('successfully updated data on user: ' . $login);
				}
				else
					die('error: wrong password');
			}
		}	
		else if ($_POST['referer'] == 'mobile')
		{
			$sql = "select password_hash, jsondata from users where login='" . $login . "' limit 1;";
			$res = mysql_query($sql);
			if (!$res || mysql_num_rows($res) == 0)
					die('fail');
			if (isset($_POST['auth']) && $_POST['auth'] == 'true') //just logging in
			{
				$value = mysql_fetch_assoc($res);
				if ($value['password_hash'] == intval($_POST['password_hash'])) //return result flag
					echo('success');
				else
					echo('fail');
			}
			else //trying to get data
			{
				$value = mysql_fetch_assoc($res);
				if ($value['password_hash'] == intval($_POST['password_hash']))
				{
					echo $value['jsondata']; //return json
				}
			}
		}
		mysql_close();
	}


?>