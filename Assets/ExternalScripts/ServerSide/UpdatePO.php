<?php

    $sqlConnection = mysqli_connect('localhost', 'test_user', 'test1234', 'urp_2020');

    if(mysqli_connect_errno()){
        echo "error in database connection";
        exit();
    }

    $player_id = $_POST['player_id'];
    $POToUpdate = $_POST['player_placeable_object'];
    
    $updateSecurityCheckQuery = "
    SELECT player_id
    FROM tbl_player_placeable_object
    WHERE player_id = '" . $player_id . "';";
    	
	$updateSecurityCheck = mysqli_query($sqlConnection, $updateSecurityCheckQuery) or die("0");
	
	if(mysqli_num_rows($updateSecurityCheck) != 1){
        echo "0";
        exit();
    }
	
    $POUpdateQuery = "
        UPDATE tbl_player_placeable_object
        SET 
            player_placeable_object_data = '" . $player_placeable_object . "'
            ,last_updated_datetime = NOW()
        WHERE player_id = '" . $player_id . "';";

    $POUpdate = mysqli_query($sqlConnection, $POUpdateQuery) or die("Update query failed");

    echo '1';

?>
