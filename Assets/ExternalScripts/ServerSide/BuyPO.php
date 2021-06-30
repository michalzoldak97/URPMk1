<?php

    $sqlConnection = mysqli_connect('localhost', 'test_user', 'test1234', 'urp_2020');

    if(mysqli_connect_errno()){
        echo "error in database connection";
        exit();
    }

    $player_id = $_POST['player_id'];
    $recieveddData = $_POST["data_to_send"];//'10^0/1/1/0/1/0|1/0/1/0/1/0';//$_POST["data_to_send"];

    $firstDataUnpack = preg_split("/[\^]/", $recieveddData);

    $playerCoinsToUpdate = (int)$firstDataUnpack[0];
    $placeableObjectData = $firstDataUnpack[1];
    
    $updateSecurityCheckQuery = "
    SELECT player_id
    FROM tbl_player_placeable_object
    WHERE player_id = '" . $player_id . "';";
    	
	$updateSecurityCheck = mysqli_query($sqlConnection, $updateSecurityCheckQuery) or die("0");
	
	if(mysqli_num_rows($updateSecurityCheck) != 1){
        echo "0";
        exit();
    }

    $playerCoinsUpdateQuery = "
        UPDATE tbl_player
        SET 
            player_coins = '" . $playerCoinsToUpdate . "'
        WHERE player_id = '" . $player_id . "';";

    $playerCoinsUpdate = mysqli_query($sqlConnection, $playerCoinsUpdateQuery) or die("Update query failed");

    $placeableObjectUpdateQuery = "
        UPDATE tbl_player_placeable_object
        SET
            player_placeable_object_data = '" . $placeableObjectData . "'
        WHERE player_id = '" . $player_id . "';";

    $placeableObjectUpdate = mysqli_query($sqlConnection, $placeableObjectUpdateQuery) or die("Update query failed");

    echo '1';

?>
