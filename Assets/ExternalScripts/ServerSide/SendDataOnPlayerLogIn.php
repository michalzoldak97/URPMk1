<?php
    $sqlConnection = mysqli_connect('localhost', 'test_user', 'test1234', 'urp_2020');

    if(mysqli_connect_errno()){
        echo "error in database connection";
        exit();
    }

    $username = $_POST["username"];//'testPass';//$_POST["username"];

    $playerIDLevelQuery = 
        "SELECT 
            CONCAT(p.player_id, '/') as player_id
            ,CONCAT(p.player_max_level, '/') as player_max_level
            ,CONCAT(p.player_coins, '/') as player_coins
            ,CONCAT(p.player_experience, '/') as player_experience
            ,CONCAT(p.player_max_slots, '^') as player_max_slots
            ,pts.task_status
            ,CONCAT('^' ,ppo.player_placeable_object_data) as player_placeable_object_data
        FROM tbl_player p
        INNER JOIN tbl_player_task_status pts           ON p.player_id = pts.player_id
        INNER JOIN tbl_player_placeable_object ppo		ON p.player_id = ppo.player_id
        WHERE player_name = '". $username ."';";

    $playerIDLevel = mysqli_query($sqlConnection, $playerIDLevelQuery) or die("Player query failed");

    $playerData = array();
    while($row = mysqli_fetch_array($playerIDLevel, MYSQLI_NUM)) { 
        $playerData[] = $row; 
    } 
    /*
    $currentPlayerId = $playerData[0][0];

    $garbageDataQuery = "SELECT player_id , player_name , log_in_attempt_datetime FROM log_tbl_player_login WHERE player_id = " . $currentPlayerId . ";";

    $farbageData = mysqli_query($sqlConnection, $garbageDataQuery) or die("Player query failed");

    foreach($playerData as $d){
        echo $d['player_id'] . '/' . $d['player_max_level'] . '|';
	}


    while($row = $farbageData -> fetch_assoc()){
    	echo $row["player_name"] . "<br />";
    } 

    while($f_row = mysqli_fetch_array($farbageData)) { 
        array_push($playerData, $f_row); 
    } 

    echo sizeof($playerData) . '<br>';

    //echo json_encode($playerData); */

    foreach($playerData as $group)
    {
        foreach($group as $d)
        {
            echo $d;
        }
    }
?>
