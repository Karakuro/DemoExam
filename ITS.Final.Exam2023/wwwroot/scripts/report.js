let template_rows = (item) => {
    return `<tr>
            <td>${item.code}</td>
            <td>${item.description}</td>
            <td>${item.total}</td>
            </tr>`
};

var reportData;

document.addEventListener("DOMContentLoaded", () => { GetData(); });

let GetData = () => {
    return fetch('/api/product/report')
        .then(response => response.json())
        .then(json => {
            reportData = json;
            InsertDataInTable(json);
        });
}

let InsertDataInTable = (data) => {
    var rows = data.map(template_rows).join('');
    document.getElementById('table_body').innerHTML = rows;
}

let FilterData = () => {
    //recupero il filtro e filtro il reportData
    let filter = getElementById("filtro").value;
    let filteredData = reportData.filter("");
    InsertDataInTable(filteredData);
}