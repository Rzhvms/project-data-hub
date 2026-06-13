import { MediaImage } from '@project-data-hub/modules/media';
import { TuiDialogContext } from '@taiga-ui/core';

export type ObjectImageModalContext = TuiDialogContext<MediaImage, ObjectImageModalInputContext>;

export type ObjectImageModalInputContext = {
    image?: MediaImage;
    fileType: string;
};
