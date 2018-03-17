$(document).ready(function () {
    ShowBitCoinAmount();
});

function ShowBitCoinAmount() {
    console.log("ShowBitCoin");
    $.ajax({ url: "/Trade/ShowBitCoin" }).done(function (response) {
        $("#bitCoin").html(response);
    });
}
