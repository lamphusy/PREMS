function delPeriod(value) {
    let table = document.getElementById(`myTable_${value}`);
  
    if (table.rows.length > 2) {
        table.deleteRow(table.rows.length - 1);
    }

}

function addPeriod(value) {
    let table = document.getElementById(`myTable_${value}`);

    var length = table.rows.length;
    var row = table.insertRow(length);
    var cell1 = row.insertCell(0);
    var cell2 = row.insertCell(1);
    var cell3 = row.insertCell(2);

    cell1.innerHTML = `<input type="text" placeholder="Enter name" class="input-size m-auto" name="name_${value}_${length}" />`;
    cell2.innerHTML = `<input type="text" placeholder="Example: 07:00" class="m-auto" name="start_${value}_${length}"/>`;
    cell3.innerHTML = `<input type="text" placeholder="Exmaple: 07:45" class="m-auto" name="end_${value}_${length}" />`;
}

var content = document.getElementById("main-content");
function addShift() {
    var length = content.childNodes.length;
    var shift = document.createElement("div");
    shift.classList.add(["card"]);
    shift.classList.add(["p-4"]);
    shift.classList.add(["mb-3"]);


    shift.innerHTML = `
                <div class="card-title mb-2 bg-white d-flex justify-content-between">
                    <input type="text" placeholder="Shift name" class="input-size form-control me-5" name="shift_${length}" required />
                    <button class="btn" type="button" data-bs-toggle="collapse" data-bs-target="#periodList_${length}" aria-expanded="false" aria-controls="periodList_${length}">
                        <i class="fa-solid fa-caret-down"></i>
                    </button>
                </div>
                <div class="card-body form-group p-0 ms-4 collapse" id="periodList_${length}">
                    <div class="">
                        <table id="myTable_${length}" class="table-responsive table">
                            <thead>
                                <tr>
                                    <th>Period name</th>
                                    <th>Start time</th>
                                    <th>End time</th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr>
                                    <th><input type="text" placeholder="Enter name" class="m-auto" name="name_${length}_1" required /></th>
                                    <th><input type="text" placeholder="Example: 07:00" class="m-auto" name="start_${length}_1" required /></th>
                                    <th><input type="text" placeholder="Exmaple: 07:45" class="m-auto" name="end_${length}_1" required /></th>
                                </tr>
                            </tbody>
                        </table>
                        <div class="d-flex">
                            <div class="button-52 btn-52-add me-3" role="button" onclick="addPeriod(${length})">+</div>
                            <div class="button-52 btn-52-del" role="button" onclick="delPeriod(${length})">-</div>
                        </div>
                    </div>
                </div>`
    content.appendChild(shift);
}

function delShift() {
    var child = content.lastChild;
    child.parentNode.removeChild(child);
}