export class tableHelper {
    static bindTableHeaderAndBodyScroll(bodyRef, headerRef) {
        bodyRef.bindScrollLeftToHeader = () => {
            headerRef.scrollLeft = bodyRef.scrollLeft;
        };
        bodyRef.addEventListener('scroll', bodyRef.bindScrollLeftToHeader);
    }
    static unbindTableHeaderAndBodyScroll(bodyRef) {
        if (bodyRef) {
            bodyRef.removeEventListener('scroll', bodyRef.bindScrollLeftToHeader);
        }
    }
}
