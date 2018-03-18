$("#BTC").keyup(function () {
    $.ajax({
        url: "Trade/GetEuro",
        type: 'POST',
        data: {
            TradeAmountBTC: $("#BTC").val(),
        },
        success: function (data) {
            $("#Euro").val(data);
        }
    });
});

$("#Euro").keyup(function () {
    $.ajax({
        url: "Trade/GetBTC",
        type: 'POST',
        data: {
            TradeAmountEuro: $("#Euro").val()
        },
        success: function (data) {
            $("#BTC").val(data);
        }
    });
});
