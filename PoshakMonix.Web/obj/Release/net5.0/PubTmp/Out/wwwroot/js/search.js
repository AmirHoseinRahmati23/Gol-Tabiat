



activeButton.addEventListener('click',(e) =>{
    document.querySelectorAll('span input[type = "checkbox"]').forEach((o) => {
        o.checked = true;
    })
    e.preventDefault();
})

disableButton.addEventListener('click',(e) =>{
    document.querySelectorAll('span input[type = "checkbox"]').forEach((o) => {
        o.checked = false;
    })
    e.preventDefault();
})