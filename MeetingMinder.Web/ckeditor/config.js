/*
Copyright (c) 2003-2012, CKSource - Frederico Knabben. All rights reserved.
For licensing, see LICENSE.html or http://ckeditor.com/license
*/

CKEDITOR.editorConfig = function( config )
{
	// Define changes to default configuration here. For example:
	// config.language = 'fr';
    // config.uiColor = '#AADC6E';

    //config.toolbar = 'Full';

    //config.toolbar_Full =
    //[
    //{ name: 'document', items: ['Source', '-', '-', 'NewPage', '-', 'Preview', 'Print', '-', '-'] },
    //{ name: 'clipboard', items: ['Cut', 'Copy', 'Paste', 'PasteText', 'PasteFromWord', '-', 'Undo', 'Redo'] },
    //{ name: 'basicstyles', items: ['Bold', 'Italic', 'Underline', 'Strike', 'Subscript', 'Superscript', '-', 'RemoveFormat', 'Center'] },
    //{ name: 'paragraph', items: ['JustifyLeft', 'JustifyCenter', 'JustifyRight', 'JustifyBlock'] }]

    config.toolbar = "CHSummaryEditor";
    config.toolbar_CHSummaryEditor = [
  ['Cut', 'Copy', 'Paste', '-', 'SpellCheck', 'Find', 'Replace', '-', 'Undo', 'Redo', '-', 'Link', 'Unlink', 'Anchor'],
  ['Bold', 'Italic', 'Underline', 'StrikeThrough', '-', 'Subscript', 'Superscript', '-', 'JustifyLeft', 'JustifyCenter', 'JustifyRight', 'JustifyFull', 'JustifyBlock', '-', 'OrderedList', 'UnorderedList', '-', 'Outdent', 'Indent', '-', 'FontFormat', '-', 'FitWindow']
    ];
    config.toolbar_CHSummaryEditor['global_xss_filtering'] = true;
};
