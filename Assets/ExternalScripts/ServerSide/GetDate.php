<?php
	
	echo "Hello, today is: " . date("Y-m-d H:i:s");
	
	$sqlConnection = mysqli_connect('localhost', 'test_user', 'test1234', 'urp_test');

    if(mysqli_connect_errno()){
        echo "error in database connection";
        exit();
    }
    $nameCheckQuery = "SELECT player_name FROM players;";
    $nameCheck = mysqli_query($sqlConnection, $nameCheckQuery) or die("error when selecting name");
    
    //$nameCheckResult = mysqli_fetch_assoc($nameCheck):array;
    
    while($row = $nameCheck -> fetch_assoc()){
    	echo $row["player_name"] . "<br />";
    }
	
?>
