<?php
$servername = "localhost";
$username = "root";
$password = "";
$dbname = "assignmentdb";

$id = $_POST["id"];
$pw = $_POST["password"];
$nick = $_POST["Nick"];
$pNum = $_POST["phonenum"];
$eM = $_POST["Email"];
$gold = $_POST["Gold"];
$lvl = $_POST["lvl"];
$Exp = $_POST["Exp"];

$conn = new mysqli($servername,
				   $username,
				   $password,
				   $dbname);

$sql = "SELECT * FROM tb_userinfo WHERE id = '" .$id. "'";
$result = $conn->query($sql);

if($result->num_rows == 0)
{
	$signup_sql = "INSERT INTO tb_userinfo (id, pw, nick, pNum, eM, gold, lvl, exp) VALUES ('" .$id. "', '" .$pw. "', '" .$nick. "', '" .$pNum. "','" .$eM. "','" .$gold. "','" .$lvl. "','" .$Exp. "')";
	$conn->query($signup_sql);
	echo "SignSuccess";
}
else 
{
	echo "DuplicateID";
}

$conn->close();
?>