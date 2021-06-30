<?php
    $sqlConnection = mysqli_connect('localhost', 'test_user', 'test1234', 'urp_2020');

    if(mysqli_connect_errno()){
        echo "error in database connection";
        exit();
    }

    $username = $_POST["username"];
    $password = $_POST["password"];

    $checkForPlayerQuery = 
    "SELECT DISTINCT
        player_name
        ,player_password_hash
        ,player_password_salt
    FROM tbl_player
    WHERE
        player_name = '" . $username ."';";
    $playerCheck = mysqli_query($sqlConnection, $checkForPlayerQuery) or die("Player check failed");
    if(mysqli_num_rows($playerCheck) != 1){
        echo "Incorrect username";
        exit();
    }

    $verifiedPlayerData = mysqli_fetch_assoc($playerCheck);

    $hashToVerify = $verifiedPlayerData["player_password_hash"];
    $saltToVerify = $verifiedPlayerData["player_password_salt"];

    $loginHash = crypt($password, $saltToVerify);
    if($hashToVerify != $loginHash)
    {
        echo "Incorrect password";
        exit();
    }

    echo "1";

    $insertLoginLogQuery = 
    "INSERT INTO log_tbl_player_login
    (
        player_id
        ,player_name
    )
    SELECT
        player_id
        ,player_name
    FROM tbl_player
    WHERE
        player_name = '" . $username ."';";
    
    $insertLogAttempt = mysqli_query($sqlConnection, $insertLoginLogQuery) or die("Log insert failed");
    
?>