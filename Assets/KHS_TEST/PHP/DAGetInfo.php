<?php
$servername = "localhost";
$username = "root";
$password = "";
$dbname = "assignmentdb";

$id = $_POST["id"];

$conn = new mysqli($servername,
				   $username,
				   $password,
				   $dbname);

if($conn->connect_error)
{
	die("Connection Failed : " . $conn->conect_error);
}


$sql = "SELECT * FROM tb_userinfo WHERE id = '" .$id. "'";
$result = $conn->query($sql);


if($result->num_rows > 0)
{
	echo "{";
	while($row = $result->fetch_assoc())
	{
        echo '"signID":"' .$row['id']. '","signPW": "' .$row['pw']. '","signNickname": "' .$row['nick']. '","signPhoneNum": "' .$row['pNum']. '","signEmail": "' .$row['eM']. '","pgold": "' .$row['gold']. '","plevel": "' .$row['lvl']. '","pexp": "' .$row['exp']. '"';
	}
	echo "}";
}

$conn->close();
?>