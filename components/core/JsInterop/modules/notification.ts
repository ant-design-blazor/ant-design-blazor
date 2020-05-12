import { removeClass, addClass, removeElement, getDom, appendHtml, htmlToElement } from "../interop";
import { getCsInstance } from "./csInstance";


const csInstanceName = "notification";
const notificationCloseFuncName = "NotificationClose";
const notificationClickFuncName = "NotificationClick";
const enterClsNames = ["ant-notification-fade-enter", "ant-notification-fade-enter-active"];
const leaveClsName = "ant-notification-fade-leave ant-notification-fade-leave-active";
const customBtnCls = ".ant-notification-notice-btn";
const closeBtnCls = ".ant-notification-notice-close";

function getNotificationService() {
    return getCsInstance(csInstanceName);
}


/**
 * remove notification box
 * @param id
 * @param element
 */
export function removeNotification(id: string, element: Element) {
    if (!element) {
        element = getDom("#" + id);
    }
    removeClass(element, enterClsNames);
    addClass(element, leaveClsName);

    window.setTimeout(() => {
        removeElement(element);
    }, 500);
    getNotificationService().invokeMethodAsync(notificationCloseFuncName, id);
}


/**
 * add notification box
 * @param htmlStr
 * @param elementSelector
 * @param id
 * @param duration
 */
export function addNotification(htmlStr, elementSelector, id, duration) {
    let spanContainer = getDom(elementSelector).children[0];
    let element = <Element>htmlToElement(htmlStr);
    appendHtml(element, spanContainer);

    let timeout;
    if (duration && duration > 0) {
        timeout = window.setTimeout(() => {
            removeNotification(id, element);
        }, duration * 1000);
    }
    //on notification box click
    element.addEventListener("click",
        () => {
            getNotificationService().invokeMethodAsync(notificationClickFuncName, id);
        });
    //on custom btn click
    let btn = getDom(customBtnCls, element);
    if (btn) {
        btn.addEventListener("click",
            e => {
                window.clearTimeout(timeout);
                removeNotification(id, element);
                e.preventDefault();
                e.stopPropagation();
            });
    }
    //on close btn click
    getDom(closeBtnCls, element)
        .addEventListener("click",
            e => {
                window.clearTimeout(timeout);
                removeNotification(id, element);
                e.preventDefault();
                e.stopPropagation();
            });
}
