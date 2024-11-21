<?php
$servername = "localhost";
$username = "root";
$password = "";
$dbname = "assignmentdb";

$id = $_POST["id"];
$gold = $_POST["gold"];
$level = $_POST["lvl"];
$exp = $_POST["exp"];

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
	$upstat_sql = "INSERT INTO tb_userinfo (id, gold, lvl, exp) VALUES ('" .$id. "', '" .$gold. "', '" .$lvl. "', '" .$exp. "')";
	$conn->query($upstat_sql);
	echo "UpdateStatusSuccess";
}

$conn->close();
?>