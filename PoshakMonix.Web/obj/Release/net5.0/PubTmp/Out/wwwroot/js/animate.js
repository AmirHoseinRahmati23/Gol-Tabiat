
// Start Mega Menu
let megaButton = document.querySelector('.megaMenuButton');
let isMegaMenuVisible = false;
megaButton.addEventListener('click', () => {
    let megaMenu = document.querySelector('.megaMenu');
    if(isMegaMenuVisible){
        megaMenu.style.display = 'none';
        isMegaMenuVisible = false;
    }else{
        megaMenu.style.display = 'flex';
        isMegaMenuVisible = true;
    }

})
// End Mega Menu



// Start Responsive Menu
let isResponsiveMenuVisible = false;

let responsiveMenuBtn = document.querySelector('.responsive-menu-button');
responsiveMenuBtn.addEventListener('click', () => {
    if(isResponsiveMenuVisible){
        document.querySelector('.responsive-menu-list').style.opacity = '0';
        setTimeout(() => {
            document.querySelector('.responsive-menu-list').style.display = 'none'; 
        },200)
        isResponsiveMenuVisible = false
    }else{
        document.querySelector('.responsive-menu-list').style.display = 'block';
        setTimeout(() => {
            document.querySelector('.responsive-menu-list').style.opacity = '1';
        },200)
        isResponsiveMenuVisible = true;
    } 
    
})

// responsive menu gender
let Gender = 'men';
document.querySelector('.men').addEventListener('click',() => {
    if(Gender == 'men'){
        document.querySelector('.responsive-mega > .man').style.height = '0';
        document.querySelector('.responsive-mega > .man').style.display = 'none';
        Gender = undefined;
    }else if( Gender == undefined){
        document.querySelector('.responsive-mega > .man').style.height = '200px';
        document.querySelector('.responsive-mega > .man').style.display = 'flex';
        Gender = 'men';
    } else {
        for(let i = 0; i<3; i++){
            document.querySelectorAll('.type')[i].style.height = '0';
            document.querySelectorAll('.type')[i].style.display = 'none';
            document.querySelector('.responsive-mega > .man').style.height = '200px';
            document.querySelector('.responsive-mega > .man').style.display = 'flex';
            Gender = 'men';
        }
    }
    
});
document.querySelector('.women').addEventListener('click',() => {
    if(Gender == 'women'){
        document.querySelector('.responsive-mega > .woman').style.height = '0';
        document.querySelector('.responsive-mega > .woman').style.display = 'none';
        Gender = undefined;
    }else if( Gender == undefined){
        document.querySelector('.responsive-mega > .woman').style.height = '200px';
        document.querySelector('.responsive-mega > .woman').style.display = 'flex';
        Gender = 'women';
    } else {
        for(let i = 0; i<3; i++){
            document.querySelectorAll('.type')[i].style.height = '0';
            document.querySelectorAll('.type')[i].style.display = 'none';
            document.querySelector('.responsive-mega > .woman').style.height = '200px';
            document.querySelector('.responsive-mega > .woman').style.display = 'flex';
            Gender = 'women';
        }
    }
    
});
document.querySelector('.children').addEventListener('click',() => {
    if(Gender == 'children'){
        document.querySelector('.responsive-mega > .child').style.display = 'none';
        document.querySelector('.responsive-mega > .child').style.height = '0';
        Gender = undefined;
    }else if( Gender == undefined){
        document.querySelector('.responsive-mega > .child').style.height = '200px';
        document.querySelector('.responsive-mega > .child').style.display = 'flex';
        Gender = 'children';
    } else {
        for(let i = 0; i<3; i++){
            document.querySelectorAll('.type')[i].style.height = '0';
            document.querySelectorAll('.type')[i].style.display = 'none';
            document.querySelector('.responsive-mega > .child').style.height = '200px';
            document.querySelector('.responsive-mega > .child').style.display = 'flex';
            Gender = 'children';
        }
    }
    
});
// responsive menu gender
// End Responsive Menu
window.onscroll = () => {
    if(window.scrollY > 500) 
    document.querySelector('.MobileMenuBar').style.opacity = '0.7';
    else document.querySelector('.MobileMenuBar').style.opacity = '1';
};