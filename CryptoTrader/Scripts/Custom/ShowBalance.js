$(document).ready(function () {
    ShowBalance();
});


function ShowBalance() {
    console.log("ShowBalance");
    $.ajax({ url: "/BankTransfer/ShowBalance" }).done(function (response) {
        $("#balance").html(response);
    });
}
