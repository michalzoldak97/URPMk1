<?php
$sqlConnection = mysqli_connect('xxx', 'xxx', 'xxx', 'xxx');

if(mysqli_connect_errno()){
echo "error in database connection";
exit();
}

$player_id = $_POST['player_id'];
$taskStatusToUpdate = $_POST['task_status'];

$taskStatusUpdateQuery = "
UPDATE tbl_player_task_status
SET
task_status = '" . $taskStatusToUpdate . "'
,player_task_status_last_updated_datetime = NOW()
WHERE player_id = '" . $player_id . "';";

$taskStatusUpdate = mysqli_query($sqlConnection, $taskStatusUpdateQuery) or die("Update query failed");

echo '1';
?>