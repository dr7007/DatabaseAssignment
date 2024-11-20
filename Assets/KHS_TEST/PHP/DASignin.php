<?php
$servername = "localhost";
$username = "root";
$password = "";
$dbname = "assignmentdb";

$id = $_POST["id"];
$pw = $_POST["pw"];

$conn = new mysqli($servername,
				   $username,
				   $password,
				   $dbname);

$sql = "SELECT * FROM tb_userinfo WHERE id = '" .$id. "'";
$result = $conn->query($sql);

if ($result->num_rows > 0)
{
	while($row = $result->fetch_assoc())
	{
		if($row["pw"] == $pw)
		{
			echo "Success";
			exit;
		}
	}
	echo "IncorrectPassword";
}
else
{
	echo "IDNotFound";
}

$conn->close();
?>