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


$sql = "SELECT * FROM tb_inveninfo WHERE InvenID = '" .$id. "'";
$result = $conn->query($sql);


if($result->num_rows > 0)
{
	while($row = $result->fetch_assoc())
	{
        echo $row['iteminfo'];
	}
}
else
{
	$create_sql = "INSERT INTO tb_inveninfo (InvenID) VALUES ('" .$id. "')";
	$conn->query($create_sql);
	echo "NewInvenIDCreat";
}


$conn->close();
?>