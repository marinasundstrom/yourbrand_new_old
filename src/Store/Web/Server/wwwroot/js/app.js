function initScrollToTop() {
    var scrollToTop = document.querySelector("#scrollToTop");
    
    if(!scrollToTop) return;

    scrollToTop.addEventListener("click", ev => {
        ev.preventDefault();
        
        window.scrollTo(0, 0);
    });
}

initScrollToTop();