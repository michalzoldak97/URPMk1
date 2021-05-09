<?php

$sqlConnection = mysqli_connect('localhost', 'test_user', 'test1234', 'urp_2020');

if(mysqli_connect_errno()){
    echo "error in database connection";
    exit();
}

$player_id = $_POST['player_id'];
$maxLevelToUpdate = $_POST['player_max_level'];

$saveMaxLevelQuery = "
    UPDATE tbl_player
    SET player_max_level = '" . $maxLevelToUpdate . "'
    WHERE player_id = '" . $player_id . "';";
$saveMaxLevel = mysqli_query($sqlConnection, $saveMaxLevelQuery) or die("0");

echo "1";

?>