<?php
$sqlConnection = mysqli_connect('###', '###', '###', '###');
if(mysqli_connect_errno()){ 
echo "error in database connection"; 
exit();
}
 
$username = $_POST["username"];
$password = $_POST["password"]; 
$nameCheckQuery = 
"
SELECT 
	player_name 
FROM tbl_player 
WHERE 
	player_name = '" . $username ."';";
$nameCheck = mysqli_query($sqlConnection, $nameCheckQuery) or die("error when selecting name");
if(mysqli_num_rows($nameCheck) > 0){ 
echo "name exists already"; 
exit(); 
} 
$passwordSalt = "\$5\$rounds=5000\$" . "larrymoemicky" . $username . "\$"; $passwordHash = crypt($password, $passwordSalt); 
$createUser = 
"INSERT INTO tbl_player (player_name, player_password_hash, player_password_salt) 
VALUES 
('" . $username . "', '" . $passwordHash . "', '" . $passwordSalt . "');";
?>
