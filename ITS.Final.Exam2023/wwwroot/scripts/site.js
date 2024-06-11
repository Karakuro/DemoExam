let SendData = () => {
    var data = {
        code: document.getElementById("productCode").value,
        quantity: document.getElementById("productQuantity").value
    };
    fetch("/api/Inventory", {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
        },
        body: JSON.stringify(data),
    }).then(response => alert("success!")).catch(err => err.JSON()).then(errMsg => console.log(errMsg));    
}