"use strict"

const uri = "/api/items";
const table = document.querySelector(".table-body");

function getData() {
    fetch(uri)
        .then(res => res.json())
        .then(data => initailizeData(data))
        .catch(error => console.error(`Error ocured while loading data ;(`, error));
}

function addItem() {
   
}

const initailizeData = (data) => {
    const { items, balance, totalIncome, totalOutcome } = data;

    console.log(items, balance, totalIncome, totalOutcome);

    for(let {name, price , type} of items) {
        table.innerHTML += 
            `<tr>
                <td>${name}</td>
                <td>${price}</td>
                <td>${type == 1 ? "Outcome" : "Income"}</td>
            </tr>`
    }
}


getData();