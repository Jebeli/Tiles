﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TileEngine.XGUI
{
    public enum Icons
    {
        NONE = 0x0,
        ENTYPO_ICON_500PX = 0x0000F100,
        ENTYPO_ICON_500PX_WITH_CIRCLE = 0x0000F101,
        ENTYPO_ICON_ADD_TO_LIST = 0x0000F102,
        ENTYPO_ICON_ADD_USER = 0x0000F103,
        ENTYPO_ICON_ADDRESS = 0x0000F104,
        ENTYPO_ICON_ADJUST = 0x0000F105,
        ENTYPO_ICON_AIR = 0x0000F106,
        ENTYPO_ICON_AIRCRAFT = 0x0000F107,
        ENTYPO_ICON_AIRCRAFT_LANDING = 0x0000F108,
        ENTYPO_ICON_AIRCRAFT_TAKE_OFF = 0x0000F109,
        ENTYPO_ICON_ALIGN_BOTTOM = 0x0000F10A,
        ENTYPO_ICON_ALIGN_HORIZONTAL_MIDDLE = 0x0000F10B,
        ENTYPO_ICON_ALIGN_LEFT = 0x0000F10C,
        ENTYPO_ICON_ALIGN_RIGHT = 0x0000F10D,
        ENTYPO_ICON_ALIGN_TOP = 0x0000F10E,
        ENTYPO_ICON_ALIGN_VERTICAL_MIDDLE = 0x0000F10F,
        ENTYPO_ICON_APP_STORE = 0x0000F110,
        ENTYPO_ICON_ARCHIVE = 0x0000F111,
        ENTYPO_ICON_AREA_GRAPH = 0x0000F112,
        ENTYPO_ICON_ARROW_BOLD_DOWN = 0x0000F113,
        ENTYPO_ICON_ARROW_BOLD_LEFT = 0x0000F114,
        ENTYPO_ICON_ARROW_BOLD_RIGHT = 0x0000F115,
        ENTYPO_ICON_ARROW_BOLD_UP = 0x0000F116,
        ENTYPO_ICON_ARROW_DOWN = 0x0000F117,
        ENTYPO_ICON_ARROW_LEFT = 0x0000F118,
        ENTYPO_ICON_ARROW_LONG_DOWN = 0x0000F119,
        ENTYPO_ICON_ARROW_LONG_LEFT = 0x0000F11A,
        ENTYPO_ICON_ARROW_LONG_RIGHT = 0x0000F11B,
        ENTYPO_ICON_ARROW_LONG_UP = 0x0000F11C,
        ENTYPO_ICON_ARROW_RIGHT = 0x0000F11D
, ENTYPO_ICON_ARROW_UP = 0x0000F11E
, ENTYPO_ICON_ARROW_WITH_CIRCLE_DOWN = 0x0000F11F
, ENTYPO_ICON_ARROW_WITH_CIRCLE_LEFT = 0x0000F120
, ENTYPO_ICON_ARROW_WITH_CIRCLE_RIGHT = 0x0000F121
, ENTYPO_ICON_ARROW_WITH_CIRCLE_UP = 0x0000F122
, ENTYPO_ICON_ATTACHMENT = 0x0000F123
, ENTYPO_ICON_AWARENESS_RIBBON = 0x0000F124
, ENTYPO_ICON_BACK = 0x0000F125
, ENTYPO_ICON_BACK_IN_TIME = 0x0000F126
, ENTYPO_ICON_BAIDU = 0x0000F127
, ENTYPO_ICON_BAR_GRAPH = 0x0000F128
, ENTYPO_ICON_BASECAMP = 0x0000F129
, ENTYPO_ICON_BATTERY = 0x0000F12A
, ENTYPO_ICON_BEAMED_NOTE = 0x0000F12B
, ENTYPO_ICON_BEHANCE = 0x0000F12C
, ENTYPO_ICON_BELL = 0x0000F12D
, ENTYPO_ICON_BLACKBOARD = 0x0000F12E
, ENTYPO_ICON_BLOCK = 0x0000F12F
, ENTYPO_ICON_BOOK = 0x0000F130
, ENTYPO_ICON_BOOKMARK = 0x0000F131
, ENTYPO_ICON_BOOKMARKS = 0x0000F132
, ENTYPO_ICON_BOWL = 0x0000F133
, ENTYPO_ICON_BOX = 0x0000F134
, ENTYPO_ICON_BRIEFCASE = 0x0000F135
, ENTYPO_ICON_BROWSER = 0x0000F136
, ENTYPO_ICON_BRUSH = 0x0000F137
, ENTYPO_ICON_BUCKET = 0x0000F138
, ENTYPO_ICON_BUG = 0x0000F139
, ENTYPO_ICON_CAKE = 0x0000F13A
, ENTYPO_ICON_CALCULATOR = 0x0000F13B
, ENTYPO_ICON_CALENDAR = 0x0000F13C
, ENTYPO_ICON_CAMERA = 0x0000F13D
, ENTYPO_ICON_CCW = 0x0000F13E
, ENTYPO_ICON_CHAT = 0x0000F13F
, ENTYPO_ICON_CHECK = 0x0000F140
, ENTYPO_ICON_CHEVRON_DOWN = 0x0000F141
, ENTYPO_ICON_CHEVRON_LEFT = 0x0000F142
, ENTYPO_ICON_CHEVRON_RIGHT = 0x0000F143
, ENTYPO_ICON_CHEVRON_SMALL_DOWN = 0x0000F144
, ENTYPO_ICON_CHEVRON_SMALL_LEFT = 0x0000F145
, ENTYPO_ICON_CHEVRON_SMALL_RIGHT = 0x0000F146
, ENTYPO_ICON_CHEVRON_SMALL_UP = 0x0000F147
, ENTYPO_ICON_CHEVRON_THIN_DOWN = 0x0000F148
, ENTYPO_ICON_CHEVRON_THIN_LEFT = 0x0000F149
, ENTYPO_ICON_CHEVRON_THIN_RIGHT = 0x0000F14A
, ENTYPO_ICON_CHEVRON_THIN_UP = 0x0000F14B
, ENTYPO_ICON_CHEVRON_UP = 0x0000F14C
, ENTYPO_ICON_CHEVRON_WITH_CIRCLE_DOWN = 0x0000F14D
, ENTYPO_ICON_CHEVRON_WITH_CIRCLE_LEFT = 0x0000F14E
, ENTYPO_ICON_CHEVRON_WITH_CIRCLE_RIGHT = 0x0000F14F
, ENTYPO_ICON_CHEVRON_WITH_CIRCLE_UP = 0x0000F150
, ENTYPO_ICON_CIRCLE = 0x0000F151
, ENTYPO_ICON_CIRCLE_WITH_CROSS = 0x0000F152
, ENTYPO_ICON_CIRCLE_WITH_MINUS = 0x0000F153
, ENTYPO_ICON_CIRCLE_WITH_PLUS = 0x0000F154
, ENTYPO_ICON_CIRCULAR_GRAPH = 0x0000F155
, ENTYPO_ICON_CLAPPERBOARD = 0x0000F156
, ENTYPO_ICON_CLASSIC_COMPUTER = 0x0000F157
, ENTYPO_ICON_CLIPBOARD = 0x0000F158
, ENTYPO_ICON_CLOCK = 0x0000F159
, ENTYPO_ICON_CLOUD = 0x0000F15A
, ENTYPO_ICON_CODE = 0x0000F15B
, ENTYPO_ICON_COG = 0x0000F15C
, ENTYPO_ICON_COLOURS = 0x0000F15D
, ENTYPO_ICON_COMPASS = 0x0000F15E
, ENTYPO_ICON_CONTROLLER_FAST_BACKWARD = 0x0000F15F
, ENTYPO_ICON_CONTROLLER_FAST_FORWARD = 0x0000F160
, ENTYPO_ICON_CONTROLLER_JUMP_TO_START = 0x0000F161
, ENTYPO_ICON_CONTROLLER_NEXT = 0x0000F162
, ENTYPO_ICON_CONTROLLER_PAUS = 0x0000F163
, ENTYPO_ICON_CONTROLLER_PLAY = 0x0000F164
, ENTYPO_ICON_CONTROLLER_RECORD = 0x0000F165
, ENTYPO_ICON_CONTROLLER_STOP = 0x0000F166
, ENTYPO_ICON_CONTROLLER_VOLUME = 0x0000F167
, ENTYPO_ICON_COPY = 0x0000F168
, ENTYPO_ICON_CREATIVE_CLOUD = 0x0000F169
, ENTYPO_ICON_CREATIVE_COMMONS = 0x0000F16A
, ENTYPO_ICON_CREATIVE_COMMONS_ATTRIBUTION = 0x0000F16B
, ENTYPO_ICON_CREATIVE_COMMONS_NODERIVS = 0x0000F16C
, ENTYPO_ICON_CREATIVE_COMMONS_NONCOMMERCIAL_EU = 0x0000F16D
, ENTYPO_ICON_CREATIVE_COMMONS_NONCOMMERCIAL_US = 0x0000F16E
, ENTYPO_ICON_CREATIVE_COMMONS_PUBLIC_DOMAIN = 0x0000F16F
, ENTYPO_ICON_CREATIVE_COMMONS_REMIX = 0x0000F170
, ENTYPO_ICON_CREATIVE_COMMONS_SHARE = 0x0000F171
, ENTYPO_ICON_CREATIVE_COMMONS_SHAREALIKE = 0x0000F172
, ENTYPO_ICON_CREDIT = 0x0000F173
, ENTYPO_ICON_CREDIT_CARD = 0x0000F174
, ENTYPO_ICON_CROP = 0x0000F175
, ENTYPO_ICON_CROSS = 0x0000F176
, ENTYPO_ICON_CUP = 0x0000F177
, ENTYPO_ICON_CW = 0x0000F178
, ENTYPO_ICON_CYCLE = 0x0000F179
, ENTYPO_ICON_DATABASE = 0x0000F17A
, ENTYPO_ICON_DIAL_PAD = 0x0000F17B
, ENTYPO_ICON_DIRECTION = 0x0000F17C
, ENTYPO_ICON_DOCUMENT = 0x0000F17D
, ENTYPO_ICON_DOCUMENT_LANDSCAPE = 0x0000F17E
, ENTYPO_ICON_DOCUMENTS = 0x0000F17F
, ENTYPO_ICON_DOT_SINGLE = 0x0000F180
, ENTYPO_ICON_DOTS_THREE_HORIZONTAL = 0x0000F181
, ENTYPO_ICON_DOTS_THREE_VERTICAL = 0x0000F182
, ENTYPO_ICON_DOTS_TWO_HORIZONTAL = 0x0000F183
, ENTYPO_ICON_DOTS_TWO_VERTICAL = 0x0000F184
, ENTYPO_ICON_DOWNLOAD = 0x0000F185
, ENTYPO_ICON_DRIBBBLE = 0x0000F186
, ENTYPO_ICON_DRIBBBLE_WITH_CIRCLE = 0x0000F187
, ENTYPO_ICON_DRINK = 0x0000F188
, ENTYPO_ICON_DRIVE = 0x0000F189
, ENTYPO_ICON_DROP = 0x0000F18A
, ENTYPO_ICON_DROPBOX = 0x0000F18B
, ENTYPO_ICON_EDIT = 0x0000F18C
, ENTYPO_ICON_EMAIL = 0x0000F18D
, ENTYPO_ICON_EMOJI_FLIRT = 0x0000F18E
, ENTYPO_ICON_EMOJI_HAPPY = 0x0000F18F
, ENTYPO_ICON_EMOJI_NEUTRAL = 0x0000F190
, ENTYPO_ICON_EMOJI_SAD = 0x0000F191
, ENTYPO_ICON_ERASE = 0x0000F192
, ENTYPO_ICON_ERASER = 0x0000F193
, ENTYPO_ICON_EVERNOTE = 0x0000F194
, ENTYPO_ICON_EXPORT = 0x0000F195
, ENTYPO_ICON_EYE = 0x0000F196
, ENTYPO_ICON_EYE_WITH_LINE = 0x0000F197
, ENTYPO_ICON_FACEBOOK = 0x0000F198
, ENTYPO_ICON_FACEBOOK_WITH_CIRCLE = 0x0000F199
, ENTYPO_ICON_FEATHER = 0x0000F19A
, ENTYPO_ICON_FINGERPRINT = 0x0000F19B
, ENTYPO_ICON_FLAG = 0x0000F19C
, ENTYPO_ICON_FLASH = 0x0000F19D
, ENTYPO_ICON_FLASHLIGHT = 0x0000F19E
, ENTYPO_ICON_FLAT_BRUSH = 0x0000F19F
, ENTYPO_ICON_FLATTR = 0x0000F1A0
, ENTYPO_ICON_FLICKR = 0x0000F1A1
, ENTYPO_ICON_FLICKR_WITH_CIRCLE = 0x0000F1A2
, ENTYPO_ICON_FLOW_BRANCH = 0x0000F1A3
, ENTYPO_ICON_FLOW_CASCADE = 0x0000F1A4
, ENTYPO_ICON_FLOW_LINE = 0x0000F1A5
, ENTYPO_ICON_FLOW_PARALLEL = 0x0000F1A6
, ENTYPO_ICON_FLOW_TREE = 0x0000F1A7
, ENTYPO_ICON_FLOWER = 0x0000F1A8
, ENTYPO_ICON_FOLDER = 0x0000F1A9
, ENTYPO_ICON_FOLDER_IMAGES = 0x0000F1AA
, ENTYPO_ICON_FOLDER_MUSIC = 0x0000F1AB
, ENTYPO_ICON_FOLDER_VIDEO = 0x0000F1AC
, ENTYPO_ICON_FORWARD = 0x0000F1AD
, ENTYPO_ICON_FOURSQUARE = 0x0000F1AE
, ENTYPO_ICON_FUNNEL = 0x0000F1AF
, ENTYPO_ICON_GAME_CONTROLLER = 0x0000F1B0
, ENTYPO_ICON_GAUGE = 0x0000F1B1
, ENTYPO_ICON_GITHUB = 0x0000F1B2
, ENTYPO_ICON_GITHUB_WITH_CIRCLE = 0x0000F1B3
, ENTYPO_ICON_GLOBE = 0x0000F1B4
, ENTYPO_ICON_GOOGLE_DRIVE = 0x0000F1B5
, ENTYPO_ICON_GOOGLE_HANGOUTS = 0x0000F1B6
, ENTYPO_ICON_GOOGLE_PLAY = 0x0000F1B7
, ENTYPO_ICON_GOOGLE_PLUS = 0x0000F1B8
, ENTYPO_ICON_GOOGLE_PLUS_WITH_CIRCLE = 0x0000F1B9
, ENTYPO_ICON_GRADUATION_CAP = 0x0000F1BA
, ENTYPO_ICON_GRID = 0x0000F1BB
, ENTYPO_ICON_GROOVESHARK = 0x0000F1BC
, ENTYPO_ICON_HAIR_CROSS = 0x0000F1BD
, ENTYPO_ICON_HAND = 0x0000F1BE
, ENTYPO_ICON_HEART = 0x0000F1BF
, ENTYPO_ICON_HEART_OUTLINED = 0x0000F1C0
, ENTYPO_ICON_HELP = 0x0000F1C1
, ENTYPO_ICON_HELP_WITH_CIRCLE = 0x0000F1C2
, ENTYPO_ICON_HOME = 0x0000F1C3
, ENTYPO_ICON_HOUR_GLASS = 0x0000F1C4
, ENTYPO_ICON_HOUZZ = 0x0000F1C5
, ENTYPO_ICON_ICLOUD = 0x0000F1C6
, ENTYPO_ICON_IMAGE = 0x0000F1C7
, ENTYPO_ICON_IMAGE_INVERTED = 0x0000F1C8
, ENTYPO_ICON_IMAGES = 0x0000F1C9
, ENTYPO_ICON_INBOX = 0x0000F1CA
, ENTYPO_ICON_INFINITY = 0x0000F1CB
, ENTYPO_ICON_INFO = 0x0000F1CC
, ENTYPO_ICON_INFO_WITH_CIRCLE = 0x0000F1CD
, ENTYPO_ICON_INSTAGRAM = 0x0000F1CE
, ENTYPO_ICON_INSTAGRAM_WITH_CIRCLE = 0x0000F1CF
, ENTYPO_ICON_INSTALL = 0x0000F1D0
, ENTYPO_ICON_KEY = 0x0000F1D1
, ENTYPO_ICON_KEYBOARD = 0x0000F1D2
, ENTYPO_ICON_LAB_FLASK = 0x0000F1D3
, ENTYPO_ICON_LANDLINE = 0x0000F1D4
, ENTYPO_ICON_LANGUAGE = 0x0000F1D5
, ENTYPO_ICON_LAPTOP = 0x0000F1D6
, ENTYPO_ICON_LASTFM = 0x0000F1D7
, ENTYPO_ICON_LASTFM_WITH_CIRCLE = 0x0000F1D8
, ENTYPO_ICON_LAYERS = 0x0000F1D9
, ENTYPO_ICON_LEAF = 0x0000F1DA
, ENTYPO_ICON_LEVEL_DOWN = 0x0000F1DB
, ENTYPO_ICON_LEVEL_UP = 0x0000F1DC
, ENTYPO_ICON_LIFEBUOY = 0x0000F1DD
, ENTYPO_ICON_LIGHT_BULB = 0x0000F1DE
, ENTYPO_ICON_LIGHT_DOWN = 0x0000F1DF
, ENTYPO_ICON_LIGHT_UP = 0x0000F1E0
, ENTYPO_ICON_LINE_GRAPH = 0x0000F1E1
, ENTYPO_ICON_LINK = 0x0000F1E2
, ENTYPO_ICON_LINKEDIN = 0x0000F1E3
, ENTYPO_ICON_LINKEDIN_WITH_CIRCLE = 0x0000F1E4
, ENTYPO_ICON_LIST = 0x0000F1E5
, ENTYPO_ICON_LOCATION = 0x0000F1E6
, ENTYPO_ICON_LOCATION_PIN = 0x0000F1E7
, ENTYPO_ICON_LOCK = 0x0000F1E8
, ENTYPO_ICON_LOCK_OPEN = 0x0000F1E9
, ENTYPO_ICON_LOG_OUT = 0x0000F1EA
, ENTYPO_ICON_LOGIN = 0x0000F1EB
, ENTYPO_ICON_LOOP = 0x0000F1EC
, ENTYPO_ICON_MAGNET = 0x0000F1ED
, ENTYPO_ICON_MAGNIFYING_GLASS = 0x0000F1EE
, ENTYPO_ICON_MAIL = 0x0000F1EF
, ENTYPO_ICON_MAIL_WITH_CIRCLE = 0x0000F1F0
, ENTYPO_ICON_MAN = 0x0000F1F1
, ENTYPO_ICON_MAP = 0x0000F1F2
, ENTYPO_ICON_MASK = 0x0000F1F3
, ENTYPO_ICON_MEDAL = 0x0000F1F4
, ENTYPO_ICON_MEDIUM = 0x0000F1F5
, ENTYPO_ICON_MEDIUM_WITH_CIRCLE = 0x0000F1F6
, ENTYPO_ICON_MEGAPHONE = 0x0000F1F7
, ENTYPO_ICON_MENU = 0x0000F1F8
, ENTYPO_ICON_MERGE = 0x0000F1F9
, ENTYPO_ICON_MESSAGE = 0x0000F1FA
, ENTYPO_ICON_MIC = 0x0000F1FB
, ENTYPO_ICON_MINUS = 0x0000F1FC
, ENTYPO_ICON_MIXI = 0x0000F1FD
, ENTYPO_ICON_MOBILE = 0x0000F1FE
, ENTYPO_ICON_MODERN_MIC = 0x0000F1FF
, ENTYPO_ICON_MOON = 0x0000F200
, ENTYPO_ICON_MOUSE = 0x0000F201
, ENTYPO_ICON_MOUSE_POINTER = 0x0000F202
, ENTYPO_ICON_MUSIC = 0x0000F203
, ENTYPO_ICON_NETWORK = 0x0000F204
, ENTYPO_ICON_NEW = 0x0000F205
, ENTYPO_ICON_NEW_MESSAGE = 0x0000F206
, ENTYPO_ICON_NEWS = 0x0000F207
, ENTYPO_ICON_NEWSLETTER = 0x0000F208
, ENTYPO_ICON_NOTE = 0x0000F209
, ENTYPO_ICON_NOTIFICATION = 0x0000F20A
, ENTYPO_ICON_NOTIFICATIONS_OFF = 0x0000F20B
, ENTYPO_ICON_OLD_MOBILE = 0x0000F20C
, ENTYPO_ICON_OLD_PHONE = 0x0000F20D
, ENTYPO_ICON_ONEDRIVE = 0x0000F20E
, ENTYPO_ICON_OPEN_BOOK = 0x0000F20F
, ENTYPO_ICON_PALETTE = 0x0000F210
, ENTYPO_ICON_PAPER_PLANE = 0x0000F211
, ENTYPO_ICON_PAYPAL = 0x0000F212
, ENTYPO_ICON_PENCIL = 0x0000F213
, ENTYPO_ICON_PHONE = 0x0000F214
, ENTYPO_ICON_PICASA = 0x0000F215
, ENTYPO_ICON_PIE_CHART = 0x0000F216
, ENTYPO_ICON_PIN = 0x0000F217
, ENTYPO_ICON_PINTEREST = 0x0000F218
, ENTYPO_ICON_PINTEREST_WITH_CIRCLE = 0x0000F219
, ENTYPO_ICON_PLUS = 0x0000F21A
, ENTYPO_ICON_POPUP = 0x0000F21B
, ENTYPO_ICON_POWER_PLUG = 0x0000F21C
, ENTYPO_ICON_PRICE_RIBBON = 0x0000F21D
, ENTYPO_ICON_PRICE_TAG = 0x0000F21E
, ENTYPO_ICON_PRINT = 0x0000F21F
, ENTYPO_ICON_PROGRESS_EMPTY = 0x0000F220
, ENTYPO_ICON_PROGRESS_FULL = 0x0000F221
, ENTYPO_ICON_PROGRESS_ONE = 0x0000F222
, ENTYPO_ICON_PROGRESS_TWO = 0x0000F223
, ENTYPO_ICON_PUBLISH = 0x0000F224
, ENTYPO_ICON_QQ = 0x0000F225
, ENTYPO_ICON_QQ_WITH_CIRCLE = 0x0000F226
, ENTYPO_ICON_QUOTE = 0x0000F227
, ENTYPO_ICON_RADIO = 0x0000F228
, ENTYPO_ICON_RAFT = 0x0000F229
, ENTYPO_ICON_RAFT_WITH_CIRCLE = 0x0000F22A
, ENTYPO_ICON_RAINBOW = 0x0000F22B
, ENTYPO_ICON_RDIO = 0x0000F22C
, ENTYPO_ICON_RDIO_WITH_CIRCLE = 0x0000F22D
, ENTYPO_ICON_REMOVE_USER = 0x0000F22E
, ENTYPO_ICON_RENREN = 0x0000F22F
, ENTYPO_ICON_REPLY = 0x0000F230
, ENTYPO_ICON_REPLY_ALL = 0x0000F231
, ENTYPO_ICON_RESIZE_100_PERCENT = 0x0000F232
, ENTYPO_ICON_RESIZE_FULL_SCREEN = 0x0000F233
, ENTYPO_ICON_RETWEET = 0x0000F234
, ENTYPO_ICON_ROCKET = 0x0000F235
, ENTYPO_ICON_ROUND_BRUSH = 0x0000F236
, ENTYPO_ICON_RSS = 0x0000F237
, ENTYPO_ICON_RULER = 0x0000F238
, ENTYPO_ICON_SAVE = 0x0000F239
, ENTYPO_ICON_SCISSORS = 0x0000F23A
, ENTYPO_ICON_SCRIBD = 0x0000F23B
, ENTYPO_ICON_SELECT_ARROWS = 0x0000F23C
, ENTYPO_ICON_SHARE = 0x0000F23D
, ENTYPO_ICON_SHARE_ALTERNATIVE = 0x0000F23E
, ENTYPO_ICON_SHAREABLE = 0x0000F23F
, ENTYPO_ICON_SHIELD = 0x0000F240
, ENTYPO_ICON_SHOP = 0x0000F241
, ENTYPO_ICON_SHOPPING_BAG = 0x0000F242
, ENTYPO_ICON_SHOPPING_BASKET = 0x0000F243
, ENTYPO_ICON_SHOPPING_CART = 0x0000F244
, ENTYPO_ICON_SHUFFLE = 0x0000F245
, ENTYPO_ICON_SIGNAL = 0x0000F246
, ENTYPO_ICON_SINA_WEIBO = 0x0000F247
, ENTYPO_ICON_SKYPE = 0x0000F248
, ENTYPO_ICON_SKYPE_WITH_CIRCLE = 0x0000F249
, ENTYPO_ICON_SLIDESHARE = 0x0000F24A
, ENTYPO_ICON_SMASHING = 0x0000F24B
, ENTYPO_ICON_SOUND = 0x0000F24C
, ENTYPO_ICON_SOUND_MIX = 0x0000F24D
, ENTYPO_ICON_SOUND_MUTE = 0x0000F24E
, ENTYPO_ICON_SOUNDCLOUD = 0x0000F24F
, ENTYPO_ICON_SPORTS_CLUB = 0x0000F250
, ENTYPO_ICON_SPOTIFY = 0x0000F251
, ENTYPO_ICON_SPOTIFY_WITH_CIRCLE = 0x0000F252
, ENTYPO_ICON_SPREADSHEET = 0x0000F253
, ENTYPO_ICON_SQUARED_CROSS = 0x0000F254
, ENTYPO_ICON_SQUARED_MINUS = 0x0000F255
, ENTYPO_ICON_SQUARED_PLUS = 0x0000F256
, ENTYPO_ICON_STAR = 0x0000F257
, ENTYPO_ICON_STAR_OUTLINED = 0x0000F258
, ENTYPO_ICON_STOPWATCH = 0x0000F259
, ENTYPO_ICON_STUMBLEUPON = 0x0000F25A
, ENTYPO_ICON_STUMBLEUPON_WITH_CIRCLE = 0x0000F25B
, ENTYPO_ICON_SUITCASE = 0x0000F25C
, ENTYPO_ICON_SWAP = 0x0000F25D
, ENTYPO_ICON_SWARM = 0x0000F25E
, ENTYPO_ICON_SWEDEN = 0x0000F25F
, ENTYPO_ICON_SWITCH = 0x0000F260
, ENTYPO_ICON_TABLET = 0x0000F261
, ENTYPO_ICON_TABLET_MOBILE_COMBO = 0x0000F262
, ENTYPO_ICON_TAG = 0x0000F263
, ENTYPO_ICON_TEXT = 0x0000F264
, ENTYPO_ICON_TEXT_DOCUMENT = 0x0000F265
, ENTYPO_ICON_TEXT_DOCUMENT_INVERTED = 0x0000F266
, ENTYPO_ICON_THERMOMETER = 0x0000F267
, ENTYPO_ICON_THUMBS_DOWN = 0x0000F268
, ENTYPO_ICON_THUMBS_UP = 0x0000F269
, ENTYPO_ICON_THUNDER_CLOUD = 0x0000F26A
, ENTYPO_ICON_TICKET = 0x0000F26B
, ENTYPO_ICON_TIME_SLOT = 0x0000F26C
, ENTYPO_ICON_TOOLS = 0x0000F26D
, ENTYPO_ICON_TRAFFIC_CONE = 0x0000F26E
, ENTYPO_ICON_TRASH = 0x0000F26F
, ENTYPO_ICON_TREE = 0x0000F270
, ENTYPO_ICON_TRIANGLE_DOWN = 0x0000F271
, ENTYPO_ICON_TRIANGLE_LEFT = 0x0000F272
, ENTYPO_ICON_TRIANGLE_RIGHT = 0x0000F273
, ENTYPO_ICON_TRIANGLE_UP = 0x0000F274
, ENTYPO_ICON_TRIPADVISOR = 0x0000F275
, ENTYPO_ICON_TROPHY = 0x0000F276
, ENTYPO_ICON_TUMBLR = 0x0000F277
, ENTYPO_ICON_TUMBLR_WITH_CIRCLE = 0x0000F278
, ENTYPO_ICON_TV = 0x0000F279
, ENTYPO_ICON_TWITTER = 0x0000F27A
, ENTYPO_ICON_TWITTER_WITH_CIRCLE = 0x0000F27B
, ENTYPO_ICON_TYPING = 0x0000F27C
, ENTYPO_ICON_UNINSTALL = 0x0000F27D
, ENTYPO_ICON_UNREAD = 0x0000F27E
, ENTYPO_ICON_UNTAG = 0x0000F27F
, ENTYPO_ICON_UPLOAD = 0x0000F280
, ENTYPO_ICON_UPLOAD_TO_CLOUD = 0x0000F281
, ENTYPO_ICON_USER = 0x0000F282
, ENTYPO_ICON_USERS = 0x0000F283
, ENTYPO_ICON_V_CARD = 0x0000F284
, ENTYPO_ICON_VIDEO = 0x0000F285
, ENTYPO_ICON_VIDEO_CAMERA = 0x0000F286
, ENTYPO_ICON_VIMEO = 0x0000F287
, ENTYPO_ICON_VIMEO_WITH_CIRCLE = 0x0000F288
, ENTYPO_ICON_VINE = 0x0000F289
, ENTYPO_ICON_VINE_WITH_CIRCLE = 0x0000F28A
, ENTYPO_ICON_VINYL = 0x0000F28B
, ENTYPO_ICON_VK = 0x0000F28C
, ENTYPO_ICON_VK_ALTERNITIVE = 0x0000F28D
, ENTYPO_ICON_VK_WITH_CIRCLE = 0x0000F28E
, ENTYPO_ICON_VOICEMAIL = 0x0000F28F
, ENTYPO_ICON_WALLET = 0x0000F290
, ENTYPO_ICON_WARNING = 0x0000F291
, ENTYPO_ICON_WATER = 0x0000F292
, ENTYPO_ICON_WINDOWS_STORE = 0x0000F293,
        ENTYPO_ICON_XING = 0x0000F294,
        ENTYPO_ICON_XING_WITH_CIRCLE = 0x0000F295,
        ENTYPO_ICON_YELP = 0x0000F296,
        ENTYPO_ICON_YOUKO = 0x0000F297,
        ENTYPO_ICON_YOUKO_WITH_CIRCLE = 0x0000F298,
        ENTYPO_ICON_YOUTUBE = 0x0000F299,
        ENTYPO_ICON_YOUTUBE_WITH_CIRCLE = 0x0000F29A

    }
}
