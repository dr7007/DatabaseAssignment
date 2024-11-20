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

$sql = "SELECT * FROM tb_userinfo WHERE id = '" .$id. "'";
$result = $conn->query($sql);

if($result->num_rows != 0)
{
	echo "DuplicateID";
}


$conn->close();
?>