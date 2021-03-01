<?php
$sqlConnection = mysqli_connect('xxx', 'xxx', 'xxx', 'xxx');

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
,CONCAT(p.player_experience, '^') as player_experience
,pts.task_status
FROM tbl_player p
INNER JOIN tbl_player_task_status pts ON p.player_id = pts.player_id
WHERE player_name = '". $username ."';";

$playerIDLevel = mysqli_query($sqlConnection, $playerIDLevelQuery) or die("Player query failed");

$playerData = array();
while($row = mysqli_fetch_array($playerIDLevel, MYSQLI_NUM)) {
$playerData[] = $row;
}
foreach($playerData as $group)
{
foreach($group as $d)
{
echo $d;
}
}
?>