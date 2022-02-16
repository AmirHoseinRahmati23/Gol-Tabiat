
let submitButton = document.querySelector('.submit');
submitButton.addEventListener('click', (e) => {
    console.log('submit');
    if (!document.querySelector('textarea').value) {

        e.preventDefault();
        document.querySelector('form').addEventListener('click', (e2) => {
            e2.preventDefault();
        })

        alert('لطفا آدرس را وارد کنید');
    }

});