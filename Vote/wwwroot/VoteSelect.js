let select_items = [
    {
        "field": "Region",
        "item": document.getElementsByName("select-region")[0],
        "status": true,
    },
    {
        "field": "Town",
        "item": document.getElementsByName("select-town")[0],
        "status": false,
    },
    {
        "field": "Street",
        "item": document.getElementsByName("select-street")[0],
        "status": false,
    },
    {
        "field": "House",
        "item": document.getElementsByName("select-house")[0],
        "status": false,
    }
]
for (let select of select_items) {
    select.item.style.display = select.status ? "block" : "none";
    select.item.onchange = (selected) => {
        console.log("called");
        let text = selected.target.options[selected.target.selectedIndex].text;
        $.ajax({
            type: 'GET',
            url: '@Url.Action("GetVotePlacesForSelect", "Vote")',
            accept: 'application/json',
            data: {
                "field": select.field,
                "text": text
            },
            success: function (places) {
                if (select_items[select_items.indexOf(select) + 1]) {
                    select_items[select_items.indexOf(select) + 1].status = true;
                    select_items[select_items.indexOf(select) + 1].item.innerHTML = '';
                    for (let place of places) {
                        let opt = document.createElement('option');
                        opt.value = place.value;
                        opt.innerHTML = place.text;
                        select_items[select_items.indexOf(select) + 1].item.appendChild(opt);
                    }
                    for (let select_inner of select_items) {
                        select_inner.item.style.display = select_inner.status ? "block" : "none";
                    }
                    select_items[select_items.indexOf(select) + 1].item.dispatchEvent(new Event("change"));
                }
            }
        });
    }
}