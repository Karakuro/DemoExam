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
    }).then(response => {
        if (response.ok) {
            alert("success");
            document.getElementById("productCode").value = "";
            document.getElementById("productQuantity").value = "";
        } else {
            alert("incorrect input");
        }
    }).catch(err => err.JSON()).then(errMsg => console.log(errMsg));
}