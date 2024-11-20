<?php
$servername = "localhost";
$username = "root";
$password = "";
$dbname = "assignmentdb";

$conn = new mysqli($servername,
				   $username,
				   $password,
				   $dbname);

if($conn->connect_error)
{
	die("Connection Failed : " . $conn->conect_error);
}


$sql = "SELECT * FROM tb_userinfo";
$result = $conn->query($sql);


if($result->num_rows > 0)
{
    $count = $result->num_rows;
    $index = 0;

	echo '{"users": [';
	while($row = $result->fetch_assoc())
	{
        echo '{"id":"' .$row['id']. '","password":"' .$row['pw']. '"}';
        if (++$index < $count) {
            echo ',';
        }
	}
	echo "]}";
}

$conn->close();
?>