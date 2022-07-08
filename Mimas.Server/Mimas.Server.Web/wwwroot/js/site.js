// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

const CustomSelectors = [
  {
    name: 'x-enable-when-selected',
    setup(itemsWithAttribute) {
      itemsWithAttribute.forEach((item) => {
        const selectorId = item.getAttribute(this.name);
        const selectorItem = document.getElementById(selectorId);

        item.disabled = selectorItem.value === '';
        selectorItem.addEventListener('change', (event) => {
          const newValue = event.target.value;
          item.disabled = newValue === '';
        });
      });
    },
  },
]

function main() {
  CustomSelectors.forEach((selector) => {
    const items = Array.from(document.querySelectorAll(`[${selector.name}]`));
    selector.setup(items);
  });
}

main();
