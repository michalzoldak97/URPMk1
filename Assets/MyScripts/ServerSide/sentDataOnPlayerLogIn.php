<?php
    $sqlConnection = mysqli_connect('localhost', 'test_user', 'test1234', 'urp_2020');

    if(mysqli_connect_errno()){
        echo "error in database connection";
        exit();
    }

    $username = $_POST["username"];

    $testPlayerQuery = "SELECT * FROM tbl_player;";

    $testPlayerCheck = mysqli_query($sqlConnection, $testPlayerQuery) or die("Player query failed");

    $testPlayerData = mysqli_fetch_assoc($testPlayerCheck);

    /*foreach($testPlayerData as $row){
		echo '<br>' . $row;
	}*/

    while($row = $testPlayerCheck -> fetch_assoc()){
    	echo $row["player_name"] . "<br />";
    } 
    
?>
