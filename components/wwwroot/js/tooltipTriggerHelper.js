export function setTooltipTriggerState(element, className, tooltipId, open, wrapped) {
  const trigger = wrapped ? element?.firstElementChild : element;
  if (!trigger) return;

  if (open) {
    const popup = document.getElementById(tooltipId)?.closest('.ant-tooltip');
    if (popup) {
      // The popup is portaled outside the docs' scoped typography container.
      popup.style.fontFamily = window.getComputedStyle(trigger).fontFamily;

      const arrow = popup.querySelector('.ant-tooltip-arrow');
      if (arrow) {
        const popupRect = popup.getBoundingClientRect();
        const triggerRect = trigger.getBoundingClientRect();
        const placement = [...popup.classList].find((name) => name.startsWith('ant-tooltip-placement-')) ?? '';
        if (placement.includes('top') || placement.includes('bottom')) {
          const left = /placement-(top|bottom)(?:left|right)/i.test(placement)
            ? triggerRect.left + triggerRect.width / 2 - popupRect.left
            : popupRect.width / 2;
          arrow.style.left = `${left}px`;
          popup.style.setProperty('--arrow-x', `${left}px`);
          popup.style.setProperty('--arrow-y', placement.includes('top')
            ? `${popupRect.height + 6}px`
            : '-6px');
        } else {
          const top = /placement-(left|right)(?:top|bottom)/i.test(placement)
            ? triggerRect.top + triggerRect.height / 2 - popupRect.top
            : popupRect.height / 2;
          arrow.style.top = `${top}px`;
          popup.style.setProperty('--arrow-x', placement.includes('left')
            ? `${popupRect.width + 6}px`
            : '-6px');
          popup.style.setProperty('--arrow-y', `${top}px`);
        }
      }
    }
    trigger.classList.add(className);
    const ids = new Set((trigger.getAttribute('aria-describedby') ?? '').split(/\s+/).filter(Boolean));
    ids.add(tooltipId);
    trigger.setAttribute('aria-describedby', [...ids].join(' '));
    return;
  }

  trigger.classList.remove(className);
  const ids = (trigger.getAttribute('aria-describedby') ?? '').split(/\s+/).filter((id) => id && id !== tooltipId);
  if (ids.length) trigger.setAttribute('aria-describedby', ids.join(' '));
  else trigger.removeAttribute('aria-describedby');
}
