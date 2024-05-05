$(function () {
    var buttonElement; // Define buttonElement outside the click event handler

    console.log("Page is ready");
    // Function to handle the Start Game button click event
    $("#startGameButton").on("click", function () {
        $.ajax({
            type: "POST",
            url: "/Game/StartGame", // Controller action to start the game
            success: function (data) {
                // Replace the content of the button zone with the returned partial view
                $(".button-zone").html(data);
                console.log("Game started successfully!");
                // Hide the start game button
                $("#startGameButton").hide();
            },
            error: function (xhr, status, error) {
                // Handle errors if needed
                console.error("Error starting game:", error);
            }
        });
    });


    $(document).on("click", ".cell-button", function (event) {
        var row = $(this).data('row');
        var col = $(this).data('col');
        buttonElement = $(this); // Store a reference to the button element

        console.log("Row: " + row + ", Column: " + col);

        switch (event.which) {
            case 1:
                event.preventDefault();
                console.log("Button at row " + row + ", column " + col + " was left clicked");
                doButtonUpdate(row, col, "/game/HandleButtonClick");
                break;
            case 2:
                alert('Middle mouse button is pressed');
                break;
            case 3:
                event.preventDefault();
                console.log("Button at row " + row + ", column " + col + " was right clicked");
                doButtonUpdate(row, col, "/game");
                break;
            default:
                alert('Nothing');
        };
    });

    // Get the current timestamp and insert it into the closest span with class "timestamp" relative to the clicked button
    $(document).on("click", ".cell-button", function (event) {
        var timestampElement = $(this).closest('.cell-button').find('.timestamp');
        var currentTime = new Date();
        timestampElement.text(currentTime.toLocaleTimeString('en-US', { hour12: false, hour: '2-digit', minute: '2-digit', second: '2-digit' }));
    });


    function doButtonUpdate(row, col, urlString) {
        $.ajax({
            dataType: "text",
            method: 'POST',
            url: urlString,
            data: {
                "row": row,
                "col": col
            },
            success: function (data) {
                console.log("Data status: " + data);
                switch (data.trim()) {
                    case "gameWon":
                        $("#gameOverOrWonContainer").load("/game/gamewon");
                        buttonElement.closest('.cell-button').load("/Game/UpdatedButton?row=" + row + "&col=" + col + "&mine=false");
                        $(".button-zone button").prop("disabled", true); // Disable all buttons in the button zone

                        break;
                    case "gameOver":
                        $("#gameOverOrWonContainer").load("/game/gameover");
                        buttonElement.closest('.cell-button').load("/Game/UpdatedButton?row=" + row + "&col=" + col + "&mine=true");
                        $(".button-zone button").prop("disabled", true); // Disable all buttons in the button zone

                        break;
                    case "continue":
                        // Update every button in the button zone
                        $(".button-zone .cell-button").each(function () {
                            var row = $(this).data('row');
                            var col = $(this).data('col');
                            $(this).load("/Game/UpdatedButton?row=" + row + "&col=" + col + "&mine=");
                        });
                        break;

                    default:
                        console.log("Unknown status: " + data);
                        break;
                }
            }
        });
    }
});
