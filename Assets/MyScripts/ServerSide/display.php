<?php

    $testString = '30^0/3/3/3';
    $playerANDposettings = preg_split("/[\^]/", $testString);
    $playerCoins = (int)$playerANDposettings[0];
    echo $playerCoins;

?>
