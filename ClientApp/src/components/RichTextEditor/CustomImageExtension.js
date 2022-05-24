import Image from '@tiptap/extension-image';

import { VueNodeViewRenderer } from '@tiptap/vue-2';

import ResizableImageTemplate from './CustomResizableImage.vue';

const CustomImage = Image.extend({

	name: 'CustomImage',

	addAttributes() {

		// Return an object with attribute configuration

		return {

			...this.parent?.(),

			src: {

				default: '',

				renderHTML: attributes => {

					// … and return an object with HTML attributes.

					return {

						src: attributes.src,

					};

				},

			},

			width: {

				default: 300,

				renderHTML: ({ width }) => ({ width }),

			},

			height: {

				default: 300,

				renderHTML: ({ height }) => ({ height }),

			},

			isDraggable: {

				default: true,

				// We don't want it to render on the img tag

				renderHTML: () => {

					return {};

				},

			},

		};

	},

	addNodeView() {

		return VueNodeViewRenderer(ResizableImageTemplate);

	},

});

export { CustomImage };

export default CustomImage;