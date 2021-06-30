<?php

    $sqlConnection = mysqli_connect('localhost', 'test_user', 'test1234', 'urp_2020');

    if(mysqli_connect_errno()){
        echo "error in database connection";
        exit();
    }

    $player_id = $_POST['player_id'];
    $taskStatusToUpdate = $_POST['task_status'];
    
    $updateSecurityCheckQuery = "
    SELECT player_id
    FROM tbl_player_task_status
    WHERE player_id = '" . $player_id . "';";
    	
	$updateSecurityCheck = mysqli_query($sqlConnection, $updateSecurityCheckQuery) or die("0");
	
	if(mysqli_num_rows($updateSecurityCheck) != 1){
        echo "0";
        exit();
    }

    $taskStatusUpdateQuery = "
        UPDATE tbl_player_task_status
        SET 
            task_status = '" . $taskStatusToUpdate . "'
            ,player_task_status_last_updated_datetime = NOW()
        WHERE player_id = '" . $player_id . "';";

    $taskStatusUpdate = mysqli_query($sqlConnection, $taskStatusUpdateQuery) or die("Update query failed");

    echo '1';

?>
