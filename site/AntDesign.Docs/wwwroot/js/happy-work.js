const targetAttribute = 'data-happy-wave-target';
const reducedMotion = window.matchMedia('(prefers-reduced-motion: reduce)');
let enabled = false;

function onClick(event) {
    if (!enabled || reducedMotion.matches) return;
    const target = event.target.closest('button, a, [role="button"], .ant-btn');
    if (!target || target.disabled || target.getAttribute('aria-disabled') === 'true') return;
    if (target.classList.contains('ant-btn-dangerous') || target.classList.contains('ant-btn-error')) return;

    const rect = target.getBoundingClientRect();
    if (!rect.width || !rect.height) return;
    target.setAttribute(targetAttribute, 'true');

    const effect = document.createElement('span');
    effect.className = 'happy-work-effect';
    effect.style.left = `${rect.left + rect.width / 2}px`;
    effect.style.top = `${rect.top + rect.height / 2}px`;
    const color = getComputedStyle(target).getPropertyValue('--ant-color-primary').trim()
        || getComputedStyle(target).color;

    for (let index = 0; index < 8; index++) {
        const dot = document.createElement('i');
        const angle = (Math.PI * 2 * index) / 8 + Math.random() * 0.3;
        const distance = 26 + Math.random() * 20;
        dot.style.setProperty('--happy-x', `${Math.cos(angle) * distance}px`);
        dot.style.setProperty('--happy-y', `${Math.sin(angle) * distance}px`);
        dot.style.setProperty('--happy-color', color);
        effect.appendChild(dot);
    }

    document.body.appendChild(effect);
    window.setTimeout(() => {
        target.removeAttribute(targetAttribute);
        effect.remove();
    }, 600);
}

document.addEventListener('click', onClick, true);

export function setEnabled(value) {
    enabled = Boolean(value);
}
