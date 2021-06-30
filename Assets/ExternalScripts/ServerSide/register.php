<?php

    $sqlConnection = mysqli_connect('localhost', 'test_user', 'test1234', 'urp_test');

    if(mysqli_connect_errno()){
        echo "error in database connection";
        exit();
    }

    $username = $_POST["username"];
    $password = $_POST["password"];

    $nameCheckQuery = "SELECT 
                            player_name 
                        FROM players
                        WHERE
                            LOWER(player_name) = LOWER('" . $username ."');";
    $nameCheck = mysqli_query($sqlConnection, $nameCheckQuery) or die("error when selecting name");
    if(mysqli_num_rows($nameCheck) > 0){
        echo "name exists already";
        exit();
    }

    $passwordSalt = "\$5\$rounds=5000\$" . "whatever" . $username . "\$";
    $passwordHash = crypt($password, $passwordSalt);
    $createUser = "INSERT INTO players (player_name, player_password_hash, player_password_salt)
                    VALUES 
                        ('" . $username . "', '" . $passwordHash . "', '" . $passwordSalt . "');";
    mysqli_query($sqlConnection, $createUser) or die("error when inserting new user");
    echo("1");

?>