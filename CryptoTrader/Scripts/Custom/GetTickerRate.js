$(document).ready(function () {
    GetRate();
});

function GetRate() {
    $.ajax({ url: "/Ticker/ShowRate" })
        .done(function (response) {
            console.log("GetRate")
            var result = "";
            var rate_old = ($("#rate").html());
            var rate_current = (response);

            $("#rate").html(rate_current);

            if (rate_current > rate_old) {
                result = ($(".rate").html("▲"));
                $(".rate").css("color", "green");
                $("#rate").css("color", "green");
            }
            else if (rate_current < rate_old) {
                result = ($(".rate").html("▼"));
                $(".rate").css("color", "red");
                $("#rate").css("color", "red");
            }
            else if (rate_current == rate_old) {
                result = ($(".rate").html("-"));
                $(".rate").css("color", "goldenrod");
                $("#rate").css("color", "goldenrod");
            }

            setTimeout(function () { GetRate(); }, 10000);
        });
}
