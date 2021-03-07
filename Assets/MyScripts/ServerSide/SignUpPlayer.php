<?php
    $sqlConnection = mysqli_connect('localhost', 'test_user', 'test1234', 'urp_2020');

    if(mysqli_connect_errno()){
        echo "error in database connection";
        exit();
    }

    $username = $_POST["username"];
    $password = $_POST["password"];

    $nameCheckQuery = "SELECT 
                            player_name 
                        FROM tbl_player
                        WHERE
                            player_name = '" . $username ."';";
    $nameCheck = mysqli_query($sqlConnection, $nameCheckQuery) or die("error when selecting name");
    if(mysqli_num_rows($nameCheck) > 0){
        echo "name exists already";
        exit();
    }

    $passwordSalt = "\$5\$rounds=5000\$" . "larrymoemicky" . $username . "\$";
    $passwordHash = crypt($password, $passwordSalt);
    $createUser = "INSERT INTO tbl_player (player_name, player_password_hash, player_password_salt)
                    VALUES 
                        ('" . $username . "', '" . $passwordHash . "', '" . $passwordSalt . "');";
    mysqli_query($sqlConnection, $createUser) or die("error when inserting new user");
    
    $createUserTask = "INSERT INTO tbl_player_task_status (player_id, task_status)
    SELECT 
    	player_id
    	,'0/0/0|0/1/0|0/2/0|0/3/0|0/4/0|1/0/0|1/1/0|1/2/0|1/3/0|1/4/0|2/0/0|2/1/0|2/2/0|2/3/0|2/4/0|3/0/0|3/1/0|3/2/0|3/3/0|3/4/0|4/0/0|4/1/0|4/2/0|4/3/0|4/4/0'
    FROM tbl_player
    WHERE player_name = '" . $username ."';";
    mysqli_query($sqlConnection, $createUserTask) or die("error when inserting new user information");
    
    
    $createUserPlaceableObject = "INSERT INTO tbl_player_placeable_object (player_id)
    SELECT player_id
    FROM tbl_player
    WHERE player_name = '" . $username ."';";
    mysqli_query($sqlConnection, $createUserPlaceableObject) or die("error when inserting new user information");
    
    echo("1");
?>
