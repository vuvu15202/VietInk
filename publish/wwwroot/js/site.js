// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

    start(); // Call your start function here
document.getElementById("cartvalue").innerHTML = 2;



function start() {
    var items = getCookie("Cart");
    var jsonCart = JSON.parse(items);
    document.getElementById("cartvalue").innerHTML = 2;
}

function getItems() {
    var itemsJSON = cookieStorage.getItem("Cart");
    if (itemsJSON) {
        return JSON.parse(itemsJSON);
    } else {
        return null;
    }
}

function getCookie(cname) {
    let name = cname + "=";
    let ca = document.cookie.split(';');
    for (let i = 0; i < ca.length; i++) {
        let c = ca[i];
        while (c.charAt(0) == ' ') {
            c = c.substring(1);
        }
        if (c.indexOf(name) == 0) {
            return c.substring(name.length, c.length);
        }
    }
    return "";
}