<?php
$servername = "localhost";
$username = "root";
$password = "";
$dbname = "assignmentdb";

$id = $_POST["id"];
$pw = $_POST["password"];

$conn = new mysqli($servername,
				   $username,
				   $password,
				   $dbname);

$sql = "SELECT * FROM tb_userinfo WHERE id = '" .$id. "'";
$result = $conn->query($sql);

if($result->num_rows == 0)
{
	$signup_sql = "INSERT INTO tb_userinfo (id, pw) VALUES ('" .$id. "', '" .$pw."')";
	$conn->query($signup_sql);
	
}
else 
{
	echo "This ID is Already Exist!!";
}

$conn->close();
?>