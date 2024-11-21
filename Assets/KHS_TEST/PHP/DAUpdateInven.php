<?php
$servername = "localhost";
$username = "root";
$password = "";
$dbname = "assignmentdb";

$id = $_POST["id"];
$itemHash = $_POST["iteminfo"];

$conn = new mysqli($servername,
				   $username,
				   $password,
				   $dbname);

if($conn->connect_error)
{
	die("Connection Failed : " . $conn->conect_error);
}


$sql = "SELECT * FROM tb_inveninfo WHERE InvenID = '" .$id. "'";
$result = $conn->query($sql);


if($result->num_rows > 0)
{
	$upstat_sql = "UPDATE tb_inveninfo SET iteminfo = '" .$itemHash. "' WHERE InvenID = '" .$id. "'";
	$conn->query($upstat_sql);
	echo "UpdateInvenSuccess";
}
else 
{
	echo "CantFindInvenID";
}


$conn->close();
?>