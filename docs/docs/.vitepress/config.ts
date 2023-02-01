import { defineConfig } from 'vitepress'


export default defineConfig({
    // shared properties and other top-level stuff...
    base:'/docs/',
    title: 'MDC-MAUI',
    description: 'Material design components for maui',
    cleanUrls: true,
    lastUpdated: true,
    locales: {
        root: {
            label: 'English',
            lang: 'en',
        },
    },
    themeConfig: {
        siteTitle: "MDC-MAUI",
        socialLinks: [
            { icon: "github", link: "https://github.com/yiszza/Material.Components.Maui" },
        ],
        editLink: {
            pattern: 'https://github.com/yiszza/Material.Components.Maui/docs/docs/:path'
        },
        sidebar: {
            '/': [
                {
                    text: 'Introduction',
                    collapsible:true,
                    items: [
                        { text: 'Getting Started', link: '/getting-started' },
                        { text: 'Tokens', link: '/tokens' },
                        { text: 'FAQ', link: '/FAQ' },
                        { text: 'Sponsor this project', link: '/sponsor' },
                    ]
                },
                {
                    text: 'Components',
                    collapsible:true,
                    items: [
                        { text: 'Button', link: '/button' },
                        { text: 'IconButton', link: '/icon-button' },
                        { text: 'Card', link: '/card' },
                        { text: 'CheckBox', link: '/checkbox' },
                        { text: 'Chip', link: '/chip' },
                        { text: 'ComboBox', link: '/combo-box' },
                        { text: 'ContextMenu', link: '/context-menu' },
                        { text: 'FAB', link: '/FAB' },
                        { text: 'NavigationBar', link: '/navigation-bar' },
                        { text: 'NavigationDrawer', link: '/navigation-drawer' },
                        { text: 'Popup', link: '/popup' },
                        { text: 'ProgressIndicator', link: '/progress-indicator' },
                        { text: 'RadioButton', link: '/radio-button' },
                        { text: 'Switch', link: '/switch' },
                        { text: 'Tabs', link: '/tabs' },
                        { text: 'TextField', link: '/text-field' },
                        { text: 'WrapLayout', link: '/wrap-layout' },
                    ]
                }
            ],
        }
    },
})