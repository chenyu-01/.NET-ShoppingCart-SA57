const inputs = document.querySelectorAll('.itemAmt');
const resultElement = document.getElementById('totalAmt');
const regex = /\-?\d+(\.\d+)?/;
var id;
function calculateTotalValue() {
    let Sum = 0;

    for (let i = 0; i < inputs.length; i++) {
        const inputValue = parseFloat(inputs[i].value);
        //const price = parseFloat(inputs[i].closest('table').querySelector('.item_price').textContent.match(regex)[0]);
        var item_code = inputs[i].id.match(regex)[0];
        const price = parseFloat(document.getElementById('price_' + item_code).textContent.match(regex)[0]);

        if (!isNaN(inputValue) && !isNaN(price)) {
            Sum += inputValue * price;
        }
    }

    resultElement.textContent = `Total: $: ${Sum}`;
}

inputs.forEach(input => {
    input.addEventListener('input', calculateTotalValue);
    input.addEventListener('blur', e => {
        const id = document.getElementById(e.target.id);
        const form = id.form;
        form.submit();
    });
    }
);



calculateTotalValue();