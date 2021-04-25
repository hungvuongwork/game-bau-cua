// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function addCommas(nStr) {
    nStr += '';
    var x = nStr.split('.');
    var x1 = x[0];
    var x2 = x.length > 1 ? '.' + x[1] : '';
    var rgx = /(\d+)(\d{3})/;
    while (rgx.test(x1)) {
        x1 = x1.replace(rgx, '$1' + ',' + '$2');
    }
    return x1 + x2;
}

// bet mascot functions
function shakeTheDice() {
    const DICE_LIMIT = 6;
    return Math.floor(Math.random() * DICE_LIMIT);
}

function diceResults() {
    let dice01 = shakeTheDice();
    let dice02 = shakeTheDice();
    let dice03 = shakeTheDice();

    return [dice01, dice02, dice03];
}

function moneyCalculate(point, userBet) {
    if (!point) return -userBet;
    return (point * userBet) + userBet;
}

function mathResult(userChoose, userBet, diceResults) {
    const DICE_RESULTS = diceResults;
    let point = 0;

    DICE_RESULTS.forEach((diceResult) => {
        if (userChoose == diceResult) point++;
    });

    return {
        point,
        result: moneyCalculate(point, userBet),
    };
}
