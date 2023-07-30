const cartOffCanvas = document.querySelector("#offcanvasRight");

function hideCartOffCanvas() {

    let offcanvas = bootstrap.Offcanvas.getInstance(cartOffCanvas);
    if(!offcanvas)
    {
        return;
    }
    offcanvas.hide();
}

export function init() {
    const navbarLinks2 = document.querySelectorAll("#offcanvasRight a")
    for(let navbarLink of navbarLinks2) {
        navbarLink.addEventListener("click", hideCartOffCanvas)
    }
}